using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// The NetworkPlayer is the class we want to write all player / client logic.
public class NetworkPlayer : Player
{
	[SerializeField] private GameObject playerObjectPrefab = null;

	public override void OnStartAuthority()
	{
		base.OnStartAuthority();

		Cmd_SpawnPlayer();
	}

	[Command]
	private void Cmd_SpawnPlayer()
	{
		NetworkServer.Spawn(Instantiate(playerObjectPrefab), connectionToClient);
	}
}
