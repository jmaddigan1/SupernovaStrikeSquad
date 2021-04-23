using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon_Energy_MiniGun : Weapon
{
	// What this weapon shoots
	[SerializeField]
	private Transform projectilePrefab = null;

	// The muzzle flash when we shoot
	[SerializeField]
	private Transform muzzelFlash = null;


	// Public Members
	// How fast does this weapon shoot
	public float FireRate = 0.1f;

	// Private Members
	// a tracker for our fire rate
	private float time = 0;


	// Increment the shooting timer
	bool IncrementTime() => (time += Time.deltaTime) > FireRate;


	// Weapon Update is called once per frame
	public override void WeaponUpdate()
	{
		if (shooting) {
			if (IncrementTime()) Shoot();
		} 
	}

	//
	private void Shoot()
	{
		time = 0f;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Cmd_SpawnProjectile(ray.GetPoint(150), transform.position);
		//if (Physics.Raycast(ray, out RaycastHit hit, 150, LayerMask.GetMask("Environment")))
		//{
		//	Cmd_SpawnProjectile(hit.point,transform.position);
		//}
		//else
		//{
		//	Cmd_SpawnProjectile(ray.GetPoint(150), transform.position);
		//}
	}

	#region Command Methods

	[Command]
	public void Cmd_SpawnProjectile(Vector3 target, Vector3 start)
	{
		GameObject go = Instantiate(projectilePrefab, start, transform.rotation).gameObject;
		go.transform.LookAt(target);

		NetworkServer.Spawn(go, connectionToClient);

		ShipController ship = GetComponentInParent<ShipController>();
		Collider projectileColliders = go.GetComponentInChildren<Collider>();
		Collider playerCollider = ship.GetCollider();

		Physics.IgnoreCollision(projectileColliders, playerCollider);


		//Rpc_SpawnMuzzelFlash();
	}
	#endregion

	#region RPC Methods

	[ClientRpc]
	public void Rpc_SpawnMuzzelFlash()
	{
		Instantiate(muzzelFlash, transform);
	}
	#endregion
}
