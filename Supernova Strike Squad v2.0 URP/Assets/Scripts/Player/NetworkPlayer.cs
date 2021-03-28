using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

	public ShipType Ship = ShipType.ShipA;

	private void Update()
	{
		// Ready UP
		if (hasAuthority)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				Ready = !Ready;
				Cmd_UpdateReady(Ready);
			}
		}
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

	#region RPC Calls

	[ClientRpc]
	public void Rpc_ChangeScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	#endregion

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
		if (playerWeaponSlotIndex < Weapons.Length) {
			Weapons[playerWeaponSlotIndex] = weaponName;
		}
	}	
	
	[Command]
	public void Cmd_UpdateShip(ShipType shipType)
	{
		Ship = shipType;
	}

	#endregion

	#endregion
}
