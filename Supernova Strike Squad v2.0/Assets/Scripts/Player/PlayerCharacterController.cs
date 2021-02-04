using System.Collections;
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
	private float RotSpeed = 125.0f;

	#endregion

	#region Client

	private void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();

		if (hasAuthority == false)
		{
			if (TryGetComponent<CharacterCamera>(out CharacterCamera camera))
			{
				Destroy(camera.gameObject);
			}
		}
	}

	public override void OnStartAuthority()
	{
		FindObjectOfType<CharacterCamera>().SetTarget(transform, cameraOffset);

		PlayerConnection.LocalPlayer.PlayerObjectManager.PlayerObject = gameObject;
	}

	private void Update()
	{
		if (hasAuthority)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (IsGrounded())
				{
					myRigidbody.AddForce(Vector3.up * 1000, ForceMode.Force);
				}
			}
		}
	}

	bool IsGrounded()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f))
		{
			Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
			return true;
		}
		else
		{
			Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
			return false;
		}
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
