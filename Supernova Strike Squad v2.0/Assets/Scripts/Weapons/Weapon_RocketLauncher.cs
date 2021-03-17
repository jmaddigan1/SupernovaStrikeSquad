using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// The Rocket Launcher is a slow dual barrel weapon that shoot tracking projectiles
public class Weapon_RocketLauncher : WeaponBase
{
	// The projectile this weapon shoots
	public GameObject ShotPrefab = null;


	// The barrels for this weapon
	public List<Transform> Barrels = new List<Transform>();


	// The fire rate of the Rocket Launcher
	public float fireRate = 0.1f;


	// Reset the shooting time when we Equip this weapon
	public override void OnEquip()
	{
		if (hasAuthority)
		{
			StartCoroutine(coShoot());
		}
	}

	// Manage shooting for the Rocket Launcher
	IEnumerator coShoot()
	{
		while (true)
		{
			if (Shooting)
			{
				CmdShoot(true);

				yield return new WaitForSecondsRealtime(fireRate);

				CmdShoot(false);

				yield return new WaitForSecondsRealtime(fireRate);
			}

			yield return null;
		}
	}

	#region Server

	[Command]
	void CmdShoot(bool left)
	{
		Transform barrel = left ? Barrels[0] : Barrels[1];

		GameObject projectile = Instantiate(ShotPrefab, barrel.position, barrel.rotation);
		NetworkServer.Spawn(projectile, connectionToClient); 
	}

	#endregion
}
