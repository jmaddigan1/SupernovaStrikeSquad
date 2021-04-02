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

	private void Update()
	{
		if (hasAuthority && CurrentWeapon) Shoot();
	}

	void Shoot()
	{
		// Check if we start or stop shooting
		if (ShipController.Interacting)
		{
			if (CurrentWeapon.GetShooting()) {
				CurrentWeapon.OnStopShooting();
			}
		}
		else
		{
			if (Input.GetKeyDown(CurrentWeapon.ShootKey))
				CurrentWeapon.OnStartShooting();

			if (Input.GetKeyUp(CurrentWeapon.ShootKey))
				CurrentWeapon.OnStopShooting();
		}	
	}

	[Command]
	public void Cmd_SpawnWeapon()
	{
		NetworkServer.Spawn(Instantiate(startingWeapon.gameObject, weaponAnchor), connectionToClient);
	}
}