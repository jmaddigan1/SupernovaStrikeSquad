﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]

public class PlayerCharacterController : NetworkBehaviour
{
	public Vector3 cameraOffset = new Vector3(0, 2.25f, -1.5f);

	// Private Members
	private Rigidbody myRigidbody;

	#region Player Stats

	private float Speed = 10.0f;
	private float RotSpeed = 75.0f;

	#endregion

	#region Client

	private void Start()
	{
		//DontDestroyOnLoad(gameObject);
		myRigidbody = GetComponent<Rigidbody>();
	}

	public override void OnStartAuthority()
	{
		FindObjectOfType<CameraController>().SetTarget(transform, cameraOffset);
		PlayerConnection.LocalPlayer.PlayerObjectManager.PlayerObject = gameObject;
	}

	void FixedUpdate()
	{
		if (hasAuthority)
		{
			Vector2 input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxisRaw("Horizontal"));


			// Move forward
			myRigidbody.MovePosition(transform.position + transform.forward * input.x * Speed * Time.fixedDeltaTime);


			// Rotate the player
			transform.Rotate(0, input.y * RotSpeed * Time.deltaTime, 0);
		}
	}

	#endregion

	#region Server


	#endregion
}
