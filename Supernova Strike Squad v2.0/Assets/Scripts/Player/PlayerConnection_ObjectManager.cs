using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
		PlayerObject = Instantiate(characterPrefab);
		NetworkServer.Spawn(PlayerObject, connectionToClient);
	}
}
