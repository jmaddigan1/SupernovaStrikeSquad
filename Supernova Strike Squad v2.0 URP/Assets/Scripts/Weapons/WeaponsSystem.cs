using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponsSystem : NetworkBehaviour
{
	public static WeaponsSystem LocalWeaponsSystem;

	[SerializeField] private Transform weaponAnchor = null;
	[SerializeField] private Weapon startingWeapon = null;

	public Weapon CurrentWeapon;

	public override void OnStartAuthority()
	{
		LocalWeaponsSystem = this;



		Cmd_SpawnWeapon();
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical("box",GUILayout.Width(150));
		GUILayout.Label("Weapon 1: " + NetworkPlayer.LocalPlayer.Self.Weapons[0].ToString());
		GUILayout.Label("Weapon 2: " + NetworkPlayer.LocalPlayer.Self.Weapons[1].ToString());
		GUILayout.EndVertical();
	}

	private void Update()
	{
		if (hasAuthority && CurrentWeapon) Shoot();
	}

	void Shoot()
	{
		// Check if we start or stop shooting

		if (Input.GetKeyDown(CurrentWeapon.ShootKey))
			CurrentWeapon.OnStartShooting();

		if (Input.GetKeyUp(CurrentWeapon.ShootKey))
			CurrentWeapon.OnStopShooting();
	}

	[Command]
	public void Cmd_SpawnWeapon()
	{
		NetworkServer.Spawn(Instantiate(startingWeapon.gameObject, weaponAnchor), connectionToClient);
	}
}