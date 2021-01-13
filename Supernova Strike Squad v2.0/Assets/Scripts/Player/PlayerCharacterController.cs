using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]

public class PlayerCharacterController : NetworkBehaviour
{
	// Public Members
	public Vector2 PlayerInput;

	// Private Members
	private Rigidbody myRigidbody;


	#region Player Stats

	private float Speed = 10.0f;
	private float RotSpeed = 75.0f;

	#endregion

	#region Client

	public override void OnStartAuthority()
	{
		FindObjectOfType<CameraController>().SetTarget(transform);
	}

	void FixedUpdate()
	{
		if (hasAuthority)
		{
			Vector2 input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxisRaw("Horizontal"));

			// Send our input to the server
			CmdUpdatePlayerInput(input);
		}

		// If we are the server we want to take the players input and update this character according
		if (isServer)
		{
			// Rotate the player
			float rot = PlayerInput.y * RotSpeed * Time.deltaTime;

			transform.Rotate(0, rot, 0);      
			

			// Move forward
			myRigidbody.MovePosition(transform.position + transform.forward * PlayerInput.x * Speed * Time.fixedDeltaTime);
		}
	}

	#endregion

	#region Server

	void Start()
	{
		if (isServer) myRigidbody = GetComponent<Rigidbody>();
	}

	[Command]
	public void CmdUpdatePlayerInput(Vector2 input)
	{
		PlayerInput = input;
	}

	#endregion
}
