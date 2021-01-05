using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
	// Editor References
	[SerializeField]
	private GameObject shipPrefab;


	// Global Members
	public static Player LocalPlayer;


	// public Members
	[HideInInspector]
	public PlayerShipController Ship;


	public override void OnStartAuthority()
	{
		LocalPlayer = this;

		SpawnShip();
	}

	[Command]
	private void SpawnShip()
	{
		NetworkServer.Spawn(Instantiate(shipPrefab), connectionToClient);
	}
}
