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

	private Vector2 velocity;

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
			weaponsSystems.CmdEquipNewWeapon(WeaponType.Minigun);
		}
	}

	void Start()
	{
		if (!isServer)
		{
			if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) Destroy(rigidbody);
		}

		if (!hasAuthority) {
			Compass.Instance?.AddTarget(transform, Color.blue);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isServer && !Paused)
		{
			velocity = Vector2.Lerp(velocity, InputVelocity, Time.deltaTime * 5);

			// Rotate / Steer
			transform.Rotate(-velocity.x * 45 * Time.deltaTime, velocity.y * 45 * Time.deltaTime, 0);

			// Move Forward
			transform.position += transform.forward * Speed * Time.deltaTime;
		}

		if (hasAuthority)
		{
			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			float y = -input.y * 5;
			float p = input.x * 10;
			float r = -input.x * 25;

			shipModel.transform.localRotation = Quaternion.Lerp(shipModel.transform.localRotation, Quaternion.Euler(y, p, r), 5 * Time.deltaTime);
		}
	}

	void FixedUpdate()
	{
		if (hasAuthority)
		{
			// Client sends input to server via SyncVar
			UpdateInput(new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")));
		}
	}

	public void PlayEnterLevel() => FindObjectOfType<ShipCamera>().PlayEnterLevel();
	public void PlayExitLevel() => FindObjectOfType<ShipCamera>().PlayExitLevel();
}
