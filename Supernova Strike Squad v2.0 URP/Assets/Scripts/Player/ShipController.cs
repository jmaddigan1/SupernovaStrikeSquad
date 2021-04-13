using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// The properties of a Ship
public class ShipData
{

}

public class ShipController : NetworkBehaviour
{
	public static bool Interacting = false;

	[SerializeField] private Transform cameraTarget = null;
	[SerializeField] private Transform shipModel = null;

	public SphereCollider PlayerCollider;
	public WeaponsSystem WeaponsSystem;

	Transform cam = null;
	Rigidbody rb = null;

	float moveMultiplier = 10.0f;

	public override void OnStartClient()
	{
		if (hasAuthority)
		{
			var moveCamera = FindObjectOfType<MoveCamera>();

			moveCamera.CameraTarget = cameraTarget;
			cam = moveCamera.transform;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.None;
		}

		if (!isServer)
		{
			PlayerCollider.enabled = false;
		}
	}

	public override void OnStartServer()
	{
		rb = GetComponent<Rigidbody>();
	}

	public Collider GetCollider()
	{
		return GetComponentInChildren<Collider>();
	}

	// Update is called once per frame
	void Update()
	{    
		// This is not the server
		// If this client is the ships owner
		if (NetworkClient.ready && hasAuthority)
		{
			UpdateMoveDirection();
			UpdateLookRotation();

			// MODEL
			UpdateModel();

			// CAMERA
			UpdateCamera();

			// BOOST
			UpdateBoost();
		}

	}

	void LateUpdate()
	{
		if (hasAuthority)
		{
			// CAMERA
			if (cam)
			{
				cam.transform.rotation = cameraTarget.rotation;
			}
		}
	}

	void FixedUpdate()
	{   
		// If this is the server update the ships with there moveDirection and targetRotation
		if (isServer && !Interacting && !ForceStop)
		{
			// MOVE FORWARD
			rb.AddRelativeForce((moveDirection * boostPower * moveMultiplier), ForceMode.Acceleration);

			// ROTATE
			rb.AddRelativeTorque(targetRotation, ForceMode.Acceleration);
		}
	}
	public bool ForceStop = false;

	#region Move Stuff
	Vector3 moveDirection;
	float moveX, moveZ, speedPercent = 1;
	float minSpeedPercent = 0.25f;
	void UpdateMoveDirection()
	{
		float deltaTime = Time.deltaTime;

		moveX = Input.GetAxis("Horizontal");
		moveZ = Input.GetAxis("Vertical");

		moveZ = moveZ + 1.0f;

		moveZ = Mathf.Clamp(moveZ, minSpeedPercent, 2.0f);

		speedPercent = Mathf.Lerp(speedPercent, Mathf.Clamp(moveZ, 0.9f, 1.5f), deltaTime * 1.0f);

		float moveSpeed = 10f;

		moveX = moveX * moveSpeed;
		moveZ = moveZ * moveSpeed;

		Cmd_UpdateMoveDirection(new Vector3(moveX, 0, moveZ));
	}

	[Command]
	public void Cmd_UpdateMoveDirection(Vector3 moveDirection)
	{
		this.moveDirection = moveDirection;
	}

	#endregion

	#region Look Stuff

	Vector3 targetRotation = Vector3.one;
	float rotX, rotY, rotZ;
	void UpdateLookRotation()
	{
		Vector2 input = GetInput() / speedPercent;

		if (Interacting) input = Vector2.zero;

		float rotSpeed = 2.5f;

		rotX = (rotSpeed * input.x);
		rotY = (rotSpeed * -input.y);

		rotZ = Input.GetAxis("Roll") * -2.5f;

		Cmd_UpdateTargetRotation(new Vector3(rotY, rotX, rotZ));
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

	[Command]
	public void Cmd_UpdateTargetRotation(Vector3 targetRotation)
	{
		this.targetRotation = targetRotation;
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

		shipRot = new Vector3(mTargetY, mTargetZ / 4f, -mTargetZ );

		shipModel.transform.localEulerAngles = shipRot;
	}

	#endregion

	#region Camera Stuff

	float cTargetX, cTargetY, cTargetZ = 1;
	void UpdateCamera()
	{
		float deltaTime = Time.deltaTime;

		Vector3 camOffset = new Vector3(0, 1.2f, -5.5f);

		cTargetX = Mathf.Lerp(cTargetX, rotX * 1.5f, deltaTime * 0.5f);
		cTargetY = Mathf.Lerp(cTargetY, rotZ * 1f, deltaTime * 0.5f);

		cTargetZ = Mathf.Lerp(cTargetZ, (speedPercent - minSpeedPercent) * 5, deltaTime * 2.5f);

		cameraTarget.localPosition = camOffset + new Vector3(cTargetX, cTargetY, -cTargetZ);
	}

	#endregion

	#region Boost Stuff

	[SyncVar]
	public bool boosting = false;
	float boostPower = 0f;
	float boostMin = 1f;
	float boostMax = 5f;
	void UpdateBoost()
	{
		// START
		if (Input.GetKeyDown(KeyCode.Space) && boosting == false) {
			OnStartBoosting();
		}

		// STOP
		if (Input.GetKeyUp(KeyCode.Space) && boosting == true) {
			OnStopBoosting();
		}

		if (isServer)
		{
			boostPower = Mathf.Lerp(boostPower, boosting ? boostMax : boostMin, Time.deltaTime * 1);
		}
	}

	void OnStartBoosting()
	{
		if (WeaponsSystem.CurrentWeapon) {
			WeaponsSystem.CurrentWeapon.OnStopShooting();
		}

		Cmd_UpdateBoost(true);
	}
	void OnStopBoosting() 
	{
		Cmd_UpdateBoost(false);
	}

	public void Cmd_UpdateBoost(bool state)
	{
		boosting = state;
	}

	#endregion
}
