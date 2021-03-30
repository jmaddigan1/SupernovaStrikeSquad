using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MOVE = ROTATION
// W and S are Speed
// A and D are z rotation

public class ShipController : MonoBehaviour
{
	[SerializeField] private Transform cam;
	[SerializeField] private Transform shipModel = null;
	[SerializeField] private Transform cameraTarget = null;

	float moveSpeed = 5f;
	float moveMultiplier = 10.0f;

	[SerializeField] float xRotation;
	[SerializeField] float yRotation;
	[SerializeField] float zRotation;
	[SerializeField] float zMovement;

	Vector3 shipRotation;

	Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();

		cam = FindObjectOfType<MoveCamera>().transform;
		cam.GetComponent<MoveCamera>().CameraTarget = cameraTarget;

		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		ReadInput();

		cam.transform.rotation = cameraTarget.rotation;
	}

	void FixedUpdate()
	{
		// MOVE FORWARD
		rb.AddRelativeForce((moveDirection * moveMultiplier) , ForceMode.Acceleration);

		// ROTATE
		rb.AddRelativeTorque(targetRotation, ForceMode.Acceleration);
	}

	float zSpeedBuildup = 1;

	float zBuildup = 0;
	float yBuildup = 0;


	#region Move Stuff
	Vector3 moveDirection;
	float moveX, moveZ;
	void UpdateMoveDirection()
	{
		moveX = Input.GetAxis("Horizontal");
		moveZ = Input.GetAxis("Vertical");

		float minSpeedPercent = 1.00f;
		float maxSpeedPercent = 1.00f;

		moveZ = Mathf.Clamp(moveZ, minSpeedPercent, maxSpeedPercent);

		float moveSpeed = 3.5f;

		moveX = moveX * moveSpeed;
		moveZ = moveZ * moveSpeed;

		moveDirection = new Vector3(moveX, 0, moveZ);
	}
	#endregion

	#region Look Stuff

	Vector3 targetRotation;
	float rotX, rotY, rotZ;
	void UpdateLookRotation()
	{
		Vector2 input = GetInput();

		float rotSpeed = 2.5f;

		rotX = (rotSpeed *  input.x);
		rotY = (rotSpeed * -input.y);

		rotZ = Input.GetAxis("Roll") * -2.5f;

		targetRotation = new Vector3(rotY, rotX, rotZ);
	}

	Vector2 GetInput()
	{
		Vector2 mousePos = Input.mousePosition;
		mousePos.x -= Screen.width / 2;
		mousePos.y -= Screen.height / 2;

		float mouseX = mousePos.x / (Screen.width / 2f);
		float mouseY = mousePos.y / (Screen.height / 2f);

		float deadZonePercent = 0.05f;

		if (Mathf.Abs(mouseX) < deadZonePercent) mouseX = 0;
		if (Mathf.Abs(mouseY) < deadZonePercent) mouseY = 0;

		float maxDist = 0.4f;

		mouseX = Mathf.Clamp(mouseX, -maxDist, maxDist);
		mouseY = Mathf.Clamp(mouseY, -maxDist, maxDist);

		return new Vector2(mouseX, mouseY);
	}

	#endregion

	#region Model Stuff

	void UpdateModel()
	{
		float yRot = rotY * 10f;
		float zRot = rotX * 20f;

		Vector3 shipRot;

		shipRot = new Vector3(yRot, zRot / 4f, -zRot);

		shipModel.transform.localEulerAngles = shipRot;
	}

	#endregion

	#region Camera Stuff

	#endregion

	void ReadInput()
	{
		UpdateMoveDirection();

		UpdateLookRotation();

		UpdateModel();

		//float deltaTime = Time.deltaTime;

		//float rotSpeed = 75;

		//Vector2 input = GetInput();

		//float x = (rotSpeed * input.x);
		//float y = (rotSpeed * -input.y);

		//float s = 5;

		//zRotation = -Input.GetAxis("Horizontal") * 50;
		//zMovement =  ((Input.GetAxis("Vertical") / 2) + 1);

		//targetRotation = new Vector3(y, x, zRotation) / 40;

		//x = x * deltaTime;
		//y = y * deltaTime;

		//#region MODEL

		//// MODEL
		//yBuildup = Mathf.Lerp(yBuildup, (rotSpeed * -input.y) * 0.2f, deltaTime * 3);
		//zBuildup = Mathf.Lerp(zBuildup, (rotSpeed * -input.x) * 0.4f, deltaTime * 3);


		//print(x);
		//shipModel.transform.localEulerAngles = new Vector3(yBuildup, 0, zBuildup);

		//#endregion

		//zSpeedBuildup = Mathf.Lerp(zSpeedBuildup, zMovement, Time.deltaTime * 3);

		//print(zSpeedBuildup * s);

		// CAMERA
		//Vector3 cameraOffset = new Vector3(0, 0.4f, -4);

		//cameraTarget.transform.localPosition = cameraOffset + (Vector3.left * zBuildup / 18) ;
	}
}
