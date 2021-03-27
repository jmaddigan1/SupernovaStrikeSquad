using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// The NetworkPlayer is the class we want to write all player / client logic.
public class NetworkPlayer : Player
{
	[SerializeField] private GameObject playerObjectPrefab = null;

	#region Client

	public override void OnStartAuthority()
	{
		base.OnStartAuthority();
		Cmd_SpawnPlayer();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (isServer) UpdateShipBays(true);
	}

	public override void OnStopClient()
	{
		base.OnStopClient();
		if (isServer) UpdateShipBays(false);
	}

	#endregion

	#region Server

	[Server]
	public void UpdateShipBays(bool starting)
	{
		foreach (ShipBay ship in FindObjectsOfType<ShipBay>())
		{
			if (ship.ownerID == ID) ship.Open = false;
		}
	}

	#region Commands

	[Command]
	private void Cmd_SpawnPlayer()
	{
		NetworkServer.Spawn(Instantiate(playerObjectPrefab), connectionToClient);
	}

	[Command]
	public void Cmd_UpdateMissionType(string[] args)
	{
		GameManager.Instance.Settings.UpdateMissionType(args);
	}

	#endregion

	#endregion
}
