using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
	// Editor References
	[SerializeField]
	private GameObject shipPrefab;

	[SerializeField]
	private GameObject characterPrefab;


	// Global Members
	public static Player LocalPlayer;


	// public Members
	[HideInInspector]
	public PlayerShipController Ship;


	public override void OnStartAuthority()
	{
		LocalPlayer = this;

		//SpawnShipIntoGames();

		// TODO:REMOVE
		SpawnPlayer();
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
}
