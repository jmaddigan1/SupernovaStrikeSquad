using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;


// The NetworkPlayer is the class we want to write all player / client logic.
public class NetworkPlayer : Player
{
	[SerializeField] private GameObject playerObjectPrefab = null;

	public WeaponTypes[] Weapons = new WeaponTypes[] {
		WeaponTypes.EnergyMiniGun, 
		WeaponTypes.RocketsLauncher
	};

	private void Update()
	{
		if (RequestPlayerData())
		{
			foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>())
			{
				int playerID = player.ID;

				Debug.Log($"[{playerID}] Player ");

				foreach (WeaponTypes weapon in Weapons) {
					Debug.Log($"Weapons " + weapon.ToString());
				}

			}
		}
	}

	bool RequestPlayerData()
	{
		if (isServer)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				return true;
			}
		}

		return false;
	}

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
			if (ship.ownerID == ID) {
				ship.Open = starting;
			} 
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

	[Command]
	public void Cmd_UpdateWeapon(WeaponTypes weaponName, int playerWeaponSlotIndex)
	{
		if (playerWeaponSlotIndex < Weapons.Length)
		{
			Weapons[playerWeaponSlotIndex] = weaponName;
		}
	}

	#endregion

	#endregion
}
