using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class PlayerConnection : NetworkBehaviour
{
	// Global Members
	public static PlayerConnection LocalPlayer;


	[Header("References")]

	[SerializeField]
	private GameObject shipPrefab = null;

	[SerializeField]
	private GameObject characterPrefab = null;


	// Private Members
	// This is the gameobject this player is currently using
	private GameObject playerObject;


	// Public Members
	// The PlayerIndex can be used to identify this player
	[SyncVar(hook = nameof(OnPlayerIDUpdate))]
	public int playerID = -1;


	// When a player connects to the game
	// We want to make sure its gameobject cannot be destroyed
	void Start() => DontDestroyOnLoad(gameObject);

	// When a player connection is destroyed we want to update the hangar states
	void OnDestroy()
	{
		if (HangarLobby.Instance) {
			HangarLobby.Instance.CloseGate(playerID);
		}
	}

	#region Client Methods

	// When the local players index is updated
	public void OnPlayerIDUpdate(int oldPlayerIndex, int newPlayerIndex)
	{
		HangarLobby.Instance.UpdateHangarStates();
	}

	// When this client is assigned a player connection script
	public override void OnStartAuthority()
	{
		// We are the local player
		LocalPlayer = this;

		// We know we are starting in the hangar scene
		// We spawn the player controller for this client
		SpawnPlayer();
	}

	// Spawns a character controller for this client
	public void SpawnPlayer() => CmdSpawnCharacterIntoGames();

	// Spawns a ship controller for this client
	public void SpawnShip() => CmdSpawnShipIntoGames();

	[ClientRpc]
	public void RpcLoadGameScene(GameData data)
	{
		LocalPlayer.StartCoroutine(coLoadGameScene());
	}

	//
	public IEnumerator coLoadGameScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

		// Wait for the scene to load
		while (!asyncLoad.isDone ) yield return null;

		yield return new WaitForSecondsRealtime(0.5f);

		LocalPlayer.SpawnShip();
	}

	[ClientRpc]
	public void RpcLoadHangarScene()
	{
		Debug.Log("RpcLoadHangarScene");
	}

	[ClientRpc]
	// Cause all the hangar doors to update
	public void RpcUpdateHangarState() => HangarLobby.Instance.UpdateHangarStates();

	#endregion

	#region Server Methods

	// The Server uses this to set a players ID
	public void SetPlayerIndex(int newID) => playerID = newID;

	[Command]
	private void CmdSpawnShipIntoGames()
	{
		playerObject = Instantiate(shipPrefab);
		NetworkServer.Spawn(playerObject, connectionToClient);
	}

	[Command]
	private void CmdSpawnCharacterIntoGames()
	{
		playerObject = Instantiate(characterPrefab);
		NetworkServer.Spawn(playerObject, connectionToClient);
	}

	[Command]
	// The players are ready and we are entering game
	public void CmdTransitionFromHangarToGame(GameData data)
	{
		RpcLoadGameScene(data);
	}

	[Command]
	// The game is over and we are moving back to the hangar
	public void CmdTransitionFromGameToHangar() => RpcLoadHangarScene();

	#endregion
}
