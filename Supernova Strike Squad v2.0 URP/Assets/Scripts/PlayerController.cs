using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(Rigidbody))]

public class PlayerController : NetworkBehaviour
{
	private Rigidbody rb;

	public static bool Interacting = false;

	[SerializeField] private Transform orientation;
	[SerializeField] private Transform cameraTarget = null;
	
	float jumpForce = 12f;

	float moveSpeed = 6f;
	float moveMultiplier = 10.0f;
	float airMultiplier = 0.2f;


	float groundDrag = 6f;
	float airDrag = 0.1f;

	public bool isGrounded;

	float horizontalMovement;
	float verticalMovement;

	Vector3 movementDirection;

	KeyCode jumpKey = KeyCode.Space;

	public override void OnStartAuthority()
	{
		FindObjectOfType<MoveCamera>().CameraTarget = cameraTarget;

		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
	}

	void Update()
	{
		if (hasAuthority)
		{
			isGrounded = Physics.Raycast(transform.position, Vector3.down, 1 + 0.1f);

			ControlDrag();
			ReadInput();

			if (Input.GetKeyDown(jumpKey) && isGrounded)
			{
				Jump();
			}
		}
	}

	void FixedUpdate()
	{
		if (hasAuthority)
		{
			if (!PlayerController.Interacting)
			{
				MovePlayer();
			}
		}
	}

	void ReadInput()
	{
		horizontalMovement = Input.GetAxisRaw("Horizontal");
		verticalMovement = Input.GetAxisRaw("Vertical");

		movementDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
	}

	void Jump()
	{
		rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
	}

	void ControlDrag()
	{
		if (isGrounded)
		{
			rb.drag = groundDrag;
		}
		else
		{
			rb.drag = airDrag;
		}
	}

	void MovePlayer()
	{
		if (isGrounded)
		{
			rb.AddForce(movementDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
		}
		else if (!isGrounded)
		{
			rb.AddForce(movementDirection.normalized * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Acceleration);
		}
	}
}
