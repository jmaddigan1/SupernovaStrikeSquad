using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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


	// Private Members
	[SyncVar] public int playerIndex;

	// NOTE: OnStartAuthority works as Start()

	// NOTE: Players are given Authority when they join the Hangar lobby
	// (This is also when they initially connect to the lobby)

	// We set the Local Player to this player and call OnHangarJoin
	public override void OnStartAuthority()
	{
		LocalPlayer = this;

		SpawnPlayer();
	}

	public override void OnStopAuthority()
	{
		base.OnStopAuthority();
	}


	// Spawns a walking play for the hangar
	public void SpawnPlayer()
	{
		// TODO: Remove the ship controller 
		//

		// TODO: Spawn the Player
		SpawnCharacterIntoGames();
	}

	// Spawns the ship this player uses
	public void SpawnShip()
	{
		// TODO: Remove the player controller 
		//

		// TODO: Spawn the Ship
		SpawnShipIntoGames();
	}
	

	#region Command Methods

	[Command]
	public void AddPlayerToServerList()
	{
		PlayerManager.Instance.AddPlayer(this);
	}

	[Command]
	private void SpawnShipIntoGames()
	{
		NetworkServer.Spawn(Instantiate(shipPrefab), connectionToClient);
	}

	[Command]
	private void SpawnCharacterIntoGames()
	{
		NetworkServer.Spawn(Instantiate(characterPrefab), connectionToClient);
	}

	#endregion
}
