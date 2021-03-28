using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
	[SerializeField] private Transform cameraTarget = null;
	[SerializeField] private Transform camera = null;

	[SerializeField] private float sensX = 100f;
	[SerializeField] private float sensY = 100f;

	float moveSpeed = 2f;
	float moveMultiplier = 10.0f;

	float mouseX;
	float mouseY;

	float multiplier =1f;

	float xRotation;
	float yRotation;

	Rigidbody rb;

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position + transform.forward * 4, 0.2f);
		Gizmos.DrawSphere(transform.position + transform.forward * 4 + transform.up * mouseY + transform.right * mouseX, 0.2f);

		Gizmos.DrawLine(transform.position + transform.forward * 4, transform.position + transform.forward * 4 + transform.up * mouseY + transform.right * mouseX);
	}

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		ReadInput();

		camera.position = cameraTarget.position;
		camera.rotation = cameraTarget.rotation;

		// float x = Input.GetAxisRaw("Horizontal");
		// float y = Input.GetAxisRaw("Vertical");

		mouseX = Input.GetAxisRaw("Horizontal");
		mouseY = Input.GetAxisRaw("Vertical");

		yRotation += mouseX * sensX * multiplier;
		xRotation -= mouseY * sensY * multiplier;

		//transform.position += transform.forward * 10 * Time.deltaTime;
	}

	void FixedUpdate()
	{
		MovePlayer();

		transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}

	void ReadInput()
	{
		//horizontalMovement = Input.GetAxisRaw("Horizontal");
		//verticalMovement = Input.GetAxisRaw("Vertical");

		//movementDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
	}

	void MovePlayer()
	{
		rb.AddForce(transform.forward * moveSpeed * moveMultiplier, ForceMode.Acceleration);
	}
}
