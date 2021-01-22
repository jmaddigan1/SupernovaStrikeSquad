using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnection : NetworkBehaviour
{
	// Global Members
	public static PlayerConnection LocalPlayer;

	// Public Members
	// The PlayerIndex can be used to identify this player
	[SyncVar(hook = nameof(OnPlayerIDUpdate))]
	public int playerID = -1;

	public PlayerConnection_SceneManager PlayerSceneManager = null;
	public PlayerConnection_ObjectManager PlayerObjectManager = null;

	// When a player connects to the game
	// We want to make sure its gameobject cannot be destroyed
	void Start() => DontDestroyOnLoad(gameObject);

	// When a player connection is destroyed we want to update the hangar states
	void OnDestroy()
	{
		if (HangarLobby.Instance)
		{
			HangarLobby.Instance.CloseGate(playerID);
		}
	}

	#region Client Methods

	// When this client is assigned a player connection script
	public override void OnStartAuthority()
	{
		// We are the local player
		LocalPlayer = this;

		// We know we are starting in the hangar scene
		// We spawn the player controller for this client
		SpawnPlayer();
	}

	// When the local players index is updated
	public void OnPlayerIDUpdate(int oldPlayerIndex, int newPlayerIndex)
	{
		HangarLobby.Instance.UpdateHangarStates();
	}

	// Spawns a character controller for this client
	public void SpawnPlayer() => PlayerObjectManager.CmdSpawnCharacterIntoGames();

	// Spawns a ship controller for this client
	public void SpawnShip() => PlayerObjectManager.CmdSpawnShipIntoGames();

	[ClientRpc]
	// Cause all the hangar doors to update
	public void RpcUpdateHangarState() => HangarLobby.Instance?.UpdateHangarStates();

	#endregion

	#region Server Methods

	// The Server uses this to set a players ID
	public void SetPlayerIndex(int newID) => playerID = newID;

	[Command]
	// The players are ready and we are entering game
	public void CmdTransitionFromHangarToGame(GameData data)
	{
		PlayerSceneManager.RpcLoadGameScene(data);
	}

	[Command]
	// The game is over and we are moving back to the hangar
	public void CmdTransitionFromGameToHangar() => PlayerSceneManager.RpcLoadHangarScene();

	[Command]
	// Start a new node event
	public void CmdStartNewEvent(int nodeID)
	{
		Debug.Log("CmdStartNewEvent: " + nodeID);

		Node node = NodeMapMenu.Instance.NodeController.NodeList[nodeID];
		NodeMapMenu.Instance.StartEvent(node.NodeData.Event);
	}

	#endregion
}
