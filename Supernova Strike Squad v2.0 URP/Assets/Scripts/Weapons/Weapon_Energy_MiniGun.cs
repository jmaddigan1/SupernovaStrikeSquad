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
			Cmd_SpawnProjectile(hit.point,transform.position);
		}
		else
		{
			Cmd_SpawnProjectile(ray.GetPoint(150), transform.position);
		}
	}

	[Command]
	public void Cmd_SpawnProjectile(Vector3 target, Vector3 start)
	{
		//SPAWN
		GameObject go = Instantiate(projectilePrefab, start, transform.rotation).gameObject;

		// ROTATE
		go.transform.LookAt(target);

		NetworkServer.Spawn(go, connectionToClient);

		// FIX COLLISION WITH OWNER
		ShipController ship = GetComponentInParent<ShipController>();
		Collider projectileColliders = go.GetComponentInChildren<Collider>();
		Collider playerCollider = ship.GetCollider();

		Physics.IgnoreCollision(projectileColliders, playerCollider);
	}
}
