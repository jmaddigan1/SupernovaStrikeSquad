using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShipController : NetworkBehaviour
{
	[SerializeField] private Transform shipModel = null;

	public float Speed = 15.0f;

	public Vector3 cameraOffset = new Vector3(0, 2, -6);

	public bool Paused = true;

	public override void OnStartAuthority()
	{
		FindObjectOfType<ShipCamera>().SetTarget(transform, cameraOffset);

		PlayerConnection.LocalPlayer.PlayerObjectManager.PlayerObject = gameObject;

		if (TryGetComponent<WeaponsSystems>(out WeaponsSystems weaponsSystems)) {
			weaponsSystems.EquipNewWeapon(WeaponType.Minigun);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (hasAuthority && !Paused)
		{
			transform.position += transform.forward * Speed * Time.deltaTime;

			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");


			transform.Rotate(-y * 45 * Time.deltaTime, x * 45 * Time.deltaTime, 0);

			float p = -Input.GetAxisRaw("Horizontal") * 25.0f;
			float r = -Input.GetAxisRaw("Vertical") * 5.0f;

			Quaternion currentRot = shipModel.transform.localRotation;
			Quaternion targetRot = Quaternion.Euler(r, 0, p);

			shipModel.transform.localRotation = Quaternion.Lerp(currentRot, targetRot, Time.deltaTime * 2);
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

	public void Pause(bool state) => Paused = state;
}
