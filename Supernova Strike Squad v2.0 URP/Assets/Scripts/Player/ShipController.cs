using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// The properties of a Ship
public class ShipData
{
	#region Movement Members

	// The speed of this ship
	public float MoveSpeed = 10f;

	// The direction we are moving.
	public Vector3 MoveDirection;

	// The ships forward momentum.
	public float MoveX;
	// The ships strafe momentum.
	public float MoveZ;

	// The percentage of our speed we are using.
	public float SpeedPercent = 1;

	// Min and Max percent speed.
	public float MinSpeedPercent = 0.7f;
	public float MaxSpeedPercent = 2.0f;

	#endregion
}

public class ShipController : NetworkBehaviour
{
	// Is the local ships owner interacting with any menus or the like?
	public static bool Interacting = false;


	// What we want our camera to move towards.
	[SerializeField] private Transform cameraTarget = null;
	// The ships model so we can add procedural animation.
	[SerializeField] private Transform shipModel = null;
	// The local players HUD.
	[SerializeField] private PlayerHUD HUD = null;


	// This ships collider / Shield sphere.
	public SphereCollider PlayerCollider;
	// This Ships Weapon Systems.
	public WeaponsSystem WeaponsSystem;
	// This Ships Stats.
	public ShipData Data = new ShipData();

	// Make the player stop moving
	public bool ForceStop = false;

	// The Scene camera.
	Transform cam = null;
	// This ships rigidbody.
	Rigidbody rb = null;


	// When the server starts.
	public override void OnStartClient()
	{
		if (hasAuthority)
		{
			var moveCamera = FindObjectOfType<MoveCamera>();

			moveCamera.CameraTarget = cameraTarget;
			cam = moveCamera.transform;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.None;

			rb = GetComponent<Rigidbody>();
		}
		else
		{
			HUD.enabled = false;
		}
	}

	private void Update()
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

			boostPower = Mathf.Lerp(boostPower, boosting ? boostMax : boostMin, Time.deltaTime * 1);
		}
	}
	private void FixedUpdate()
	{   
		// If this is the server update the ships with there moveDirection and targetRotation
		//if (hasAuthority && !Interacting && !ForceStop)
		if (hasAuthority)
		{
			// MOVE FORWARD
			rb.AddRelativeForce((Data.MoveDirection * boostPower * 10), ForceMode.Acceleration);

			// ROTATE
			rb.AddRelativeTorque(targetRotation, ForceMode.Acceleration);
		}
	}
	private void LateUpdate()
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


	#region Move Stuff

	void UpdateMoveDirection()
	{
		// Read Input from keyboard.
		Data.MoveX = Input.GetAxis("Horizontal");
		Data.MoveZ = Input.GetAxis("Vertical");

		// Remap the input to positive values. (0f - 2f)
		Data.MoveZ = Data.MoveZ + 1.0f;

		// Clamp the new ship momentum to its Min and Max value. (0.3f - 2.0f)
		Data.MoveZ = Mathf.Clamp(Data.MoveZ, Data.MinSpeedPercent, Data.MaxSpeedPercent);


		// Calculate the percent of our speed we are using.
		// And Smooth it towards the target value
		Data.SpeedPercent = Mathf.Lerp(Data.SpeedPercent, Mathf.Clamp(Data.MoveZ, Data.MinSpeedPercent, Data.MaxSpeedPercent), DeltaTime);


		// Multiply our speed percent with 
		Data.MoveX = Data.MoveX * Data.MoveSpeed;
		Data.MoveZ = Data.MoveZ * Data.MoveSpeed;


		// Update our move direction for the local player
		Data.MoveDirection = new Vector3(Data.MoveX, 0, Data.MoveZ);
	}

	#endregion
	#region Look Stuff

	Vector3 targetRotation = Vector3.one;
	float rotX, rotY, rotZ;
	void UpdateLookRotation()
	{
		Vector2 input = GetInput() / Data.SpeedPercent;

		if (Interacting) input = Vector2.zero;

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

		cTargetZ = Mathf.Lerp(cTargetZ, (Data.SpeedPercent - Data.MinSpeedPercent) * 5, deltaTime * 2.5f);

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


	// Utility Methods
	// Get this ships collider.
	public Collider GetCollider() => GetComponentInChildren<Collider>();

	// Properties
	float DeltaTime { get { return Time.deltaTime; } }
}
