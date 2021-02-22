using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerShipController : NetworkBehaviour
{
	[SerializeField] private Transform shipModel = null;

	// Public Members
	// The speed this ship moves forward
	public float Speed = 15.0f;

	public Vector3 cameraOffset = new Vector3(0, 2, -6);
	public Vector2 InputVelocity;

	public bool Paused = true;

	[Command]
	public void UpdateInput(Vector3 newInput) => InputVelocity = newInput;

	public override void OnStartAuthority()
	{
		// Set the camera settings
		FindObjectOfType<ShipCamera>().SetTarget(transform, cameraOffset);

		// Set the player object
		PlayerConnection.LocalPlayer.Object.PlayerObject = gameObject;

		// Equip our first weapon
		if (TryGetComponent<WeaponsSystems>(out WeaponsSystems weaponsSystems)) {
			weaponsSystems.EquipNewWeapon(WeaponType.Minigun);
		}
	}

	void Start()
	{
		if (!isServer)
		{
			if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) Destroy(rigidbody);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isServer && !Paused)
		{
			// Move Forward
			transform.position += transform.forward * Speed * Time.deltaTime;

			// Rotate / Steer
			transform.Rotate(-InputVelocity.x * 45 * Time.deltaTime, InputVelocity.y * 45 * Time.deltaTime, 0);
		}

		if (hasAuthority)
		{
			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));

			float y = -input.y * 5;
			float p = input.x * 10;
			float r = -input.x * 25;

			shipModel.transform.localRotation = Quaternion.Lerp(shipModel.transform.localRotation, Quaternion.Euler(y, p, r), 5 * Time.deltaTime);
		}
	}

	public void PlayEnterLevel()
	{
		FindObjectOfType<ShipCamera>().PlayEnterLevel();
	}
	public void PlayExitLevel()
	{
		FindObjectOfType<ShipCamera>().PlayExitLevel();
	}

	void FixedUpdate()
	{
		if (hasAuthority)
		{
			// Client sends input to server via SyncVar
			UpdateInput(new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")));
		}
	}

	void OnDestroy()
	{
		if (hasAuthority)
		{
			var camera = FindObjectOfType<CameraController>();
			if (camera) camera.SetTarget(null, cameraOffset);
		}
	}
}
