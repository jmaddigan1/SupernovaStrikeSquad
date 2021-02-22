using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

[RequireComponent(typeof(NodeMapDepthController))]
[RequireComponent(typeof(NodeMapNodeController))]

// The Node Map Menu is a canvas spawned in by each client when the Main scene is loaded
// This Menu loads the game that was selected in the hangar

public class NodeMapMenu : NetworkBehaviour
{
	public static NodeMapMenu Instance;

	[SerializeField]
	// The DepthController manages the depth groups for the node map
	public NodeMapDepthController DepthController = null;

	[SerializeField]
	// The NodeController manages the nodes for the node map
	public NodeMapNodeController NodeController = null;

	[SerializeField] private GameObject EnemySpawnerPrefab = null;
	[SerializeField] private GameObject LevelGeneratorPrefab = null;

	[SerializeField]
	private Node NodePrafab = null;

	[SerializeField]
	private GameObject ContentAnchor = null;

	public NodeMapData CurrentNodeMap_Client;
	public NodeMapData CurrentNodeMap_Server;

	// Private Members
	// Is there currently a node event running on the server?
	private bool eventRunning = false;

	#region GUI / Debug


	private void OnGUI()
	{
		if (isServer == false) return;

		float width = 300;

		GUILayout.BeginHorizontal();
		GUILayout.Space(Screen.width - width);
		GUILayout.BeginVertical("box", GUILayout.Width(width));

		GUILayout.Label("NodeMap Menu Editor");

		GUIRegion(() => {

			GUILayout.Label("Options");

			GUIRegion(() => {

				GUILayout.Label("Node Settings");

				if (GUILayout.Button("Kill all Enemies")) {

					foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
						enemy.gameObject.GetComponent<Health>().DealDamage(100000);
					}
				}

			});

			GUIRegion(() => {

				GUILayout.Label("Enemy Settings");

			});

		});

		GUIRegion(() => {

			GUILayout.Label("Settings");

		});

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	void GUIRegion(Action action)
	{
		GUI.color = new Color(1, 1, 1, 0.25f);

		GUILayout.BeginVertical("box");

		GUI.color = new Color(1, 1, 1, 1);

		action.Invoke();

		GUILayout.EndVertical();
	}

	#endregion

	#region Server

	// When the NodeMap is first created we want to load the map we are playing ON THE SERVER
	IEnumerator Start()
	{
		if (isServer)
		{
			NetworkServer.Spawn(Instantiate(LevelGeneratorPrefab));
			NetworkServer.Spawn(Instantiate(EnemySpawnerPrefab));
		}

		while (LevelGenerator.Instance == null || EnemySpawner.Instance == null) yield return null;

		// Initialize the singleton
		if (Instance == null) { Instance = this; }
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		// If this is the server or the host
		if (isServer)
		{
			// Get the NodeMap data from something
			CurrentNodeMap_Server = Campaigns.TestCampaign();

			// Give each node its index
			for (int i = 0; i < CurrentNodeMap_Server.Nodes.Count; i++) CurrentNodeMap_Server.Nodes[i].Index = i;

			// NOTE: Because of the NodeEvent in NodeMapData -> Node -> NodeEvent
			// We cannot send the normal map data class over the server
			// Mirror does not let us use abstract classes as data

			// So as a fix, we are sending the node data as a string
			// This will lose all the node event data, however the clients don't need to know this anyways
			string dataJson = JsonUtility.ToJson(CurrentNodeMap_Server);

			yield return new WaitForSeconds(0.5f);

			PlayerConnection.LocalPlayer.Map.RpcBroadcastNewNodeMap(dataJson);
		}
	}

	[Server]
	public void StartEvent(NodeEvent eventData)
	{
		StartCoroutine(StartNewEvent(eventData));
	}

	[Server]
	public void OnCompletedNode(bool wasCompleted)
	{
		if (wasCompleted)
		{
			Debug.Log("NODE CONPLETED!: You can move on");

			CurrentNodeMap_Server.CurrentDepth++;

			if (CurrentNodeMap_Server.Completed())
			{
				OnNodeMapCompleted();
				Debug.Log("Node Map Completed!");
				return;
			}
		}
		else
		{
			Debug.Log("NODE FAILED!: Retry?");
		}

		string dataJson = JsonUtility.ToJson(CurrentNodeMap_Server);

		PlayerConnection.LocalPlayer.Map.RpcBroadcastNewNodeMap(dataJson);
	}

	[Server]
	public void OnNodeMapCompleted()
	{
		LevelGenerator.Remove();

		LobbyManager.Instance.EndGame();
	}

	#region Manage Event

	[Server]
	IEnumerator StartNewEvent(NodeEvent eventData)
	{
		if (!eventRunning && isServer)
		{
			eventRunning = true;

			yield return CloseMenu();

			yield return PlayEvent(eventData);

			eventRunning = false;

			yield return OpenMenu();
		}
	}

	[Server]
	IEnumerator PlayEvent(NodeEvent eventData)
	{
		eventData.OnStartEvent();
		
		yield return UnpausePlayers(eventData);

		LevelGenerator.Build(eventData.Environment);

		while (eventData.IsOver() == false)
		{
			yield return null;
		}

		yield return PausePlayers();

		eventData.OnEndEvent();

		OnCompletedNode(true);

		LevelGenerator.Remove();
	}

	[Server]
	IEnumerator OpenMenu()
	{     
		// Tell the client
		// To Open the NodeMap menu
		RpcOpenMenu();

		//  Wait for the expected animation time for the player ship to fly out
		yield return new WaitForSeconds(1.2f);
	}

	[Server]
	IEnumerator CloseMenu()
	{
		// Tell the client
		// To Close the NodeMap menu
		RpcCloseMenu();

		// Wait for the expected animation time for the player ship to fly in
		yield return new WaitForSeconds(1.2f);
	}

	[Server]
	IEnumerator UnpausePlayers(NodeEvent eventData)
	{     
		// UnPause all players
		RpcOnUnpausePlayer();
		foreach (var player in FindObjectsOfType<PlayerShipController>())
		{
			player.transform.position = new Vector3(0, 0, -eventData.Environment.Size);
			player.Paused = false;
		}

		yield return null;
	}

	[Server]
	IEnumerator PausePlayers()
	{    
		// Pause all players
		RpcOnPausePlayer();
		foreach (var player in FindObjectsOfType<PlayerShipController>())
		{
			player.Paused = true;
		}

		yield return null;
	}

	#endregion

	#endregion

	#region Client
	// The server has told us we have a new NodeMap
	// We want to load that map and begin a new game
	[Client]
	public void GenerateNodeMap(string mapDataJson)
	{
		NodeController.Clear();
		DepthController.Clear();

		// We have received the node map data from the server
		// There is a problem however

		// Because the Node events are ABSTRACT, we are missing all our event data

		// This is OK because we only need to send the ID of the node we want to play to the server
		// clients don't need to know all the information for each events
		CurrentNodeMap_Client = JsonUtility.FromJson<NodeMapData>(mapDataJson);

		DepthController.Init(CurrentNodeMap_Client.Depth);

		foreach (NodeData node in CurrentNodeMap_Client.Nodes)
		{
			NodeController.NodeList.Add(Instantiate(NodePrafab, DepthController.GetDepthAnchor(node.Depth)).Init(this, node));
		}
	}

	[ClientRpc]
	public void RpcOnPausePlayer()
	{
		PlayerConnection.LocalPlayer.Object.PlayerExitLevelAnimation();
	}

	[ClientRpc]
	public void RpcOnUnpausePlayer()
	{
		PlayerConnection.LocalPlayer.Object.PlayerEnterLevelAnimation();
	}

	[ClientRpc]
	public void RpcOpenMenu() => StartCoroutine(CoOpenMenu());
	IEnumerator CoOpenMenu()
	{
		ContentAnchor.SetActive(true);

		bool waiting = true;

		Tween.Instance.EaseOut_Transform_ElasticY(ContentAnchor.transform, Screen.height, 0, 1.0f, 0, () => { waiting = false; });

		while (waiting) yield return null;
	}

	[ClientRpc]
	public void RpcCloseMenu() => StartCoroutine(CoCloseMenu());
	IEnumerator CoCloseMenu()
	{
		bool waiting = true;

		Tween.Instance.EaseIn_Transform_ElasticY(ContentAnchor.transform, 0, Screen.height, 1.0f, 0, () => { waiting = false; });

		while (waiting) yield return null;

		ContentAnchor.SetActive(false);
	}

	#endregion
}
