using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon_Energy_MiniGun : Weapon
{
	[SerializeField] private Transform projectilePrefab = null;

	public float fireRate = 0.1f;

	float time = 0;

	public override void WeaponUpdate()
	{
		if (shooting)
		{
			if (IncrementTime()) Shoot();
		}
	}

	bool IncrementTime()
	{
		return (time += Time.deltaTime) > fireRate;
	}

	void Shoot()
	{
		time = 0f;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 150, LayerMask.GetMask("Environment")))
		{
			Cmd_SpawnProjectile(hit.point);
		}
		else
		{
			Cmd_SpawnProjectile(ray.GetPoint(150));
		}
	}

	[Command]
	public void Cmd_SpawnProjectile(Vector3 target)
	{
		//SPAWN
		GameObject go = Instantiate(projectilePrefab, transform.position, transform.rotation).gameObject;
		NetworkServer.Spawn(go);

		// ROTATE
		go.transform.LookAt(target);

		// FIX COLLISION WITH OWNER
		ShipController ship = GetComponentInParent<ShipController>();
		Collider projectileColliders = go.GetComponentInChildren<Collider>();
		Collider playerCollider = ship.GetCollider();

		Physics.IgnoreCollision(projectileColliders, playerCollider);
	}
}
