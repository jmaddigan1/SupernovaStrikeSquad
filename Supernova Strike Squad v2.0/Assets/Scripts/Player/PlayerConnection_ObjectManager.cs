using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerConnection_ObjectManager : NetworkBehaviour
{
	[Header("References")]

	[SerializeField]
	public GameObject shipPrefab = null;

	[SerializeField]
	public GameObject characterPrefab = null;

	[Header("Player Object")]

	public GameObject PlayerObject;

	[Command]
	public void CmdSpawnShipIntoGames()
	{
		PlayerObject = Instantiate(shipPrefab);
		NetworkServer.Spawn(PlayerObject, connectionToClient);
	}

	[Command]
	public void CmdSpawnCharacterIntoGames()
	{
		Transform go = PlayerConnection.LocalPlayer.gameObject.transform;

		Debug.Log(PlayerConnection.LocalPlayer.playerID);

		PlayerObject = Instantiate(characterPrefab);
		NetworkServer.Spawn(PlayerObject, connectionToClient);
	}

	public void PlayerEnterLevelAnimation()
	{
		if (PlayerObject.TryGetComponent<PlayerShipController>(out PlayerShipController shipController))
		{
			shipController.PlayEnterLevel();
		}
	}	
	public void PlayerExitLevelAnimation()
	{
		if (PlayerObject.TryGetComponent<PlayerShipController>(out PlayerShipController shipController))
		{
			shipController.PlayExitLevel();
		}
	}
}
