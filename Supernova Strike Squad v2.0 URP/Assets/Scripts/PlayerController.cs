using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;

	[SerializeField] private Transform orientation;

	[Header("Movement")]
	public float MoveSpeed = 6f;
	float moveMultiplier = 10.0f;
	[SerializeField] private float airMultiplier = 0.1f;

	[Header("Jumping")]
	public float JumpForce = 15f;

	[Header("Keybinds")]
	[SerializeField] KeyCode jumpKey = KeyCode.Space;

	float groundDrag = 6f;
	float airDrag = 0.1f;

	public bool isGrounded;


	float horizontalMovement;
	float verticalMovement;

	Vector3 movementDirection;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
	}

	void Update()
	{
		isGrounded = Physics.Raycast(transform.position, Vector3.down, 1 + 0.1f);

		ControlDrag();
		ReadInput();

		if (Input.GetKeyDown(jumpKey) && isGrounded)
		{
			Jump();
		}
	}
	void FixedUpdate()
	{
		MovePlayer();
	}

	void Jump()
	{
		rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
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

	void ReadInput()
	{
		horizontalMovement = Input.GetAxisRaw("Horizontal");
		verticalMovement = Input.GetAxisRaw("Vertical");

		movementDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
	}

	void MovePlayer()
	{
		if (isGrounded)
		{
			rb.AddForce(movementDirection.normalized * MoveSpeed * moveMultiplier, ForceMode.Acceleration);
		}
		else if (!isGrounded)
		{
			rb.AddForce(movementDirection.normalized * MoveSpeed * moveMultiplier * airMultiplier, ForceMode.Acceleration);
		}
	}


}
