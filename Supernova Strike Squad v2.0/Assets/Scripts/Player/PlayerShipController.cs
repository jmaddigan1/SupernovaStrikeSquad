﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShipController : NetworkBehaviour
{
	public float Speed = 15.0f;

	public Vector3 cameraOffset = new Vector3(0, 2, -6);

	public override void OnStartAuthority()
	{
		FindObjectOfType<CameraController>().SetTarget(transform, cameraOffset);

		PlayerConnection.LocalPlayer.PlayerObjectManager.PlayerObject = gameObject;

		if (TryGetComponent<WeaponsSystems>(out WeaponsSystems weaponsSystems)) {
			weaponsSystems.EquipNewWeapon(WeaponType.Minigun);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (hasAuthority)
		{
			transform.position += transform.forward * Speed * Time.deltaTime; 

			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			transform.Rotate(-y * 45 * Time.deltaTime, x * 45 * Time.deltaTime, 0);
		}
	}

	void OnDestroy()
	{
		if (hasAuthority)
		{
			FindObjectOfType<CameraController>().SetTarget(null, cameraOffset);
		}
	}
}
