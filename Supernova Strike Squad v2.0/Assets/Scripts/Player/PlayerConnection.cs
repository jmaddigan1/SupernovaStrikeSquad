using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerConnection : NetworkBehaviour
{
	// Global Members
	public static PlayerConnection LocalPlayer;


	// Editor References
	[SerializeField]
	private GameObject shipPrefab = null;

	[SerializeField]
	private GameObject characterPrefab = null;


	// public Members
	[HideInInspector]
	public PlayerShipController Ship;

	[SyncVar(hook = nameof(UpdateHangar))] 
	public int playerIndex = -1;


	#region Client

	public void SetPlayerIndex(int newIndex) => playerIndex = newIndex;

	public void UpdateHangar(int oldPlayerIndex, int newPlayerIndex) {
		HangarLobby.Instance.UpdateHangarStates();
	}

	public override void OnStartAuthority()
	{
		LocalPlayer = this;
		SpawnPlayer();
	}

	// Spawns a walking play for the hangar
	public void SpawnPlayer()
	{
		// TODO: Remove the ship controller 
		//

		// TODO: Spawn the Player
		CmdSpawnCharacterIntoGames();
	}

	// Spawns the ship this player uses
	public void SpawnShip()
	{
		// TODO: Remove the player controller 
		//

		// TODO: Spawn the Ship
		CmdSpawnShipIntoGames();
	}

	void OnDestroy()
	{
		HangarLobby.Instance.CloseGate(playerIndex);
	}

	#endregion

	#region Server

	[Command]
	private void CmdSpawnShipIntoGames() => 
		NetworkServer.Spawn(Instantiate(shipPrefab), connectionToClient);

	[Command]
	private void CmdSpawnCharacterIntoGames() => 
		NetworkServer.Spawn(Instantiate(characterPrefab), connectionToClient);

	[Command]
	public void CmdUpdateGameMode(GameModeType gameMode) =>
		GameManager.Instance.GameMode = gameMode;

	[Command]
	public void CmdStartGame() => 
		GameManager.Instance.RpcStartGame();
	
	[ClientRpc]
	public void RpcUpdateHangarState() => 	
		HangarLobby.Instance.UpdateHangarStates();

	#endregion
}
