using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// The Mini Gun is a single barrel fast firing weapon
// It is the default weapon for the player
public class Weapon_Minigun : WeaponBase
{
	// The projectile this weapon shoots
	public GameObject ShotPrefab = null;


	// The barrel for this weapon
	public Transform Barrel = null;


	// The fire rate of the Mini-gun
	public float fireRate = 0.1f;


	// A timer to keep track of the time between shots
	private float time;


	// Reset the shooting time when we Equip or Unequip this weapon
	public override void OnEquip() => time = 0;
	public override void OnUnequip() => time = 0;


	// Set the timer to 0
	void ResetTimer() => time = 0.0f;

	// Manage shooting for the Mini-gun
	void FixedUpdate()
    {
		// Make sure only the local client can use this
		if (hasAuthority == false) return;

		if (Shooting)
		{
			time += Time.fixedDeltaTime;

			if (time > fireRate)
			{
				CmdShoot();
				ResetTimer();
			}
		}
    }

	public override void OnShootUp()
	{
		base.OnShootUp();

		// When we stop shooting we want to reset the timer
		ResetTimer();
	}

	#region Server

	[Command]
	void CmdShoot()
	{
		GameObject projectile = Instantiate(ShotPrefab, Barrel.position, Barrel.rotation);

		NetworkServer.Spawn(projectile, connectionToClient);
	}

	#endregion
}
