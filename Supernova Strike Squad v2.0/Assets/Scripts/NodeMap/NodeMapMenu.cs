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

	public NodeMapData CurrentNodeMap { get; private set; }

	// Private Members
	// Is there currently a node event running on the server?
	private bool eventRunning = false;

	#region Client
	// The server has told us we have a new NodeMap
	// We want to load that map and begin a new game
	[ClientRpc]
	public void RpcGenerateNodeMap(string mapDataJson)
	{
		NodeController.Clear();
		DepthController.Clear();

		// We have received the node map data from the server
		// There is a problem however

		// Because the Node events are ABSTRACT, we are missing all our event data

		// This is OK because we only need to send the ID of the node we want to play to the server
		// clients don't need to know all the information for each events
		CurrentNodeMap = JsonUtility.FromJson<NodeMapData>(mapDataJson);

		DepthController.Init(CurrentNodeMap.Depth);

		foreach (NodeData node in CurrentNodeMap.Nodes)
		{
			NodeController.NodeList.Add(Instantiate(NodePrafab, DepthController.GetDepthAnchor(node.Depth)).Init(this, node));
		}
	}

	[ClientRpc] public void RpcOpenMenu() => ContentAnchor.SetActive(true);
	[ClientRpc] public void RpcCloseMenu() => ContentAnchor.SetActive(false);

	[ClientRpc] public void RpcPausePlayer() => PlayerConnection.LocalPlayer.PlayerObjectManager.PausePlayerObject();
	[ClientRpc] public void RpcUnpausePlayer() => PlayerConnection.LocalPlayer.PlayerObjectManager.UnpausePlayerObject();

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
			NodeMapData mapData = new NodeMapData
			{
				Depth = 6,

				Nodes = new List<NodeData> {
				new NodeData { Name = "TempName 0", Depth = 0, Event = new NodeEvent_Arean(),  ConnectedNodes = new List<int>{ 1, 2 } },

				new NodeData { Name = "TempName 1", Depth = 1, ConnectedNodes = new List<int>{ 3 } },
				new NodeData { Name = "TempName 2", Depth = 1, ConnectedNodes = new List<int>{ 3 } },

				new NodeData { Name = "TempName 3", Depth = 2, ConnectedNodes = new List<int>{ 4, 7 } },

				new NodeData { Name = "TempName 4", Depth = 3, ConnectedNodes = new List<int>{ 5, 6, 7 } },

				new NodeData { Name = "TempName 5", Depth = 4, ConnectedNodes = new List<int>{ 8 } },
				new NodeData { Name = "TempName 6", Depth = 4, ConnectedNodes = new List<int>{ 8 } },
				new NodeData { Name = "TempName 7", Depth = 4, ConnectedNodes = new List<int>{ 8 } },

				new NodeData { Name = "TempName 8", Depth = 5, ConnectedNodes = new List<int>{ } },
			}
			};

			// Give each node its index
			for (int i = 0; i < mapData.Nodes.Count; i++) mapData.Nodes[i].Index = i;

			// NOTE: Because of the NodeEvent in NodeMapData -> Node -> NodeEvent
			// We cannot send the normal map data class over the server
			// Mirror does not let us use abstract classes as data

			// So as a fix, we are sending the node data as a string
			// This will lose all the node event data, however the clients don't need to know this anyways
			string dataJson = JsonUtility.ToJson(mapData);

			RpcGenerateNodeMap(dataJson); 
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

			CurrentNodeMap.CurrentDepth++;

			if (CurrentNodeMap.Completed())
			{
				Debug.Log("Node Map Completed!");
				return;
			}
		}
		else
		{
			Debug.Log("NODE FAILED!: Retry?");
		}

		string dataJson = JsonUtility.ToJson(CurrentNodeMap); 

		RpcGenerateNodeMap(dataJson);
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

		RpcUnpausePlayer();

		LevelGenerator.Build(eventData.Environment);

		while (eventData.IsOver() == false)
		{
			yield return null;
		}

		RpcPausePlayer();

		eventData.OnEndEvent();

		OnCompletedNode(true);

		LevelGenerator.Remove();
	}

	[Server]
	IEnumerator OpenMenu()
	{
		RpcOpenMenu();
		yield return new WaitForSeconds(0.5f);
	}

	[Server]
	IEnumerator CloseMenu()
	{
		RpcCloseMenu();
		yield return new WaitForSeconds(0.5f);
	}

	#endregion

	#endregion
}
