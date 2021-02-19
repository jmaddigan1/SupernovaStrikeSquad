using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon_Minigun : WeaponBase
{
	[SerializeField]
	private GameObject shot = null;

    public float fireRate = 0.1f;

    private float timer;

	[SerializeField]
	private bool shooting;

	public override void OnShootUp() => shooting = false;
	public override void OnShootDown() => shooting = true;

	public override void OnEquip()
	{
		timer = 0.0f;
	}	

	void FixedUpdate()
    {
        if (hasAuthority == false) return;

		if (shooting)
		{
			timer += Time.fixedDeltaTime;

			if (timer > fireRate)
			{
				CmdShoot();
				timer = 0.0f;
			}
		}
		else
		{
			timer = 0.0f;
		}	
    }

	[Command]
	void CmdShoot()
	{
		GameObject projectile = Instantiate(shot, Barrels[0].position, Barrels[0].rotation);

		NetworkServer.Spawn(projectile, connectionToClient);
	}
}
