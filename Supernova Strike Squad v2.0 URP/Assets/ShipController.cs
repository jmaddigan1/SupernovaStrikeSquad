using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MOVE = ROTATION
// W and S are Speed
// A and D are z rotation

public class ShipController : MonoBehaviour
{

	[SerializeField] private Transform shipModel = null;
	[SerializeField] private Transform cameraTarget = null;

	Transform cam = null;
	Rigidbody rb = null;

	float moveMultiplier = 10.0f;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		cam = FindObjectOfType<MoveCamera>().transform;

		cam.GetComponent<MoveCamera>().CameraTarget = cameraTarget;

		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		UpdateMoveDirection();
		UpdateLookRotation();

		UpdateModel();

		UpdateCamera();

		cam.transform.rotation = cameraTarget.rotation;
	}

	void FixedUpdate()
	{
		// MOVE FORWARD
		rb.AddRelativeForce((moveDirection * moveMultiplier), ForceMode.Acceleration);

		// ROTATE
		rb.AddRelativeTorque(targetRotation, ForceMode.Acceleration);
	}

	#region Move Stuff
	Vector3 moveDirection;
	float moveX, moveZ, speedPercent;
	float minSpeedPercent = 0.9f;
	void UpdateMoveDirection()
	{
		float deltaTime = Time.deltaTime;

		moveX = Input.GetAxis("Horizontal");
		moveZ = Input.GetAxis("Vertical");

		moveZ = moveZ + 1.0f;

		float minSpeedPercent = 0.25f;
		float maxSpeedPercent = 2.00f;

		moveZ = Mathf.Clamp(moveZ, minSpeedPercent, maxSpeedPercent);

		speedPercent = Mathf.Lerp(speedPercent, Mathf.Clamp(moveZ, 0.9f, 1.5f), deltaTime * 1.0f);

		float moveSpeed = 5f;

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
		Vector2 input = GetInput() / speedPercent;

		float rotSpeed = 2.5f;

		rotX = (rotSpeed * input.x);
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

	float mTargetY, mTargetZ;
	void UpdateModel()
	{
		float deltaTime = Time.deltaTime;

		mTargetY = Mathf.Lerp(mTargetY, rotY * 10f, deltaTime * 2.5f);
		mTargetZ = Mathf.Lerp(mTargetZ, rotX * 20f, deltaTime * 2.5f);

		Vector3 shipRot;

		shipRot = new Vector3(mTargetY, mTargetZ / 4f, -mTargetZ);

		shipModel.transform.localEulerAngles = shipRot;
	}

	#endregion

	#region Camera Stuff

	float cTargetX, cTargetY, cTargetZ = 1;
	void UpdateCamera()
	{
		float deltaTime = Time.deltaTime;

		Vector3 camOffset = new Vector3(0, 0.9f, -5.5f);

		cTargetX = Mathf.Lerp(cTargetX, rotX * 1f, deltaTime * 0.5f);
		cTargetY = Mathf.Lerp(cTargetY, rotZ * 1f, deltaTime * 0.5f);

		cTargetZ = Mathf.Lerp(cTargetZ, (speedPercent - minSpeedPercent) * 5, deltaTime * 2.5f);

		cameraTarget.localPosition = camOffset + new Vector3(cTargetX, cTargetY, -cTargetZ);
	}

	#endregion
}
