using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
	North,
	NorthEast,
	East,
	SouthEast,
	South,
	SouthWest,
	West,
	NorthWest
}

public class TrackPlayerState : FSMState
{
	//// Direction Dictionary
	//public Dictionary<Directions, Vector2> DirectionDictionary =
	//new Dictionary<Directions, Vector2>() {

	//	{ Directions.North,     new Vector2( 0,  1) },
	//	{ Directions.NorthEast, new Vector2( 1,  1) },
	//	{ Directions.East,      new Vector2( 1,  0) },
	//	{ Directions.SouthEast, new Vector2( 1, -1) },
	//	{ Directions.South,     new Vector2( 0, -1) },
	//	{ Directions.SouthWest, new Vector2(-1, -1) },
	//	{ Directions.West,      new Vector2(-1,  0) },
	//	{ Directions.NorthWest, new Vector2(-1,  1) },

	//};

	[SerializeField] private Transform target = null;

	public GameObject Self;

	public bool ShouldDodge = false;

	Vector2 Size = new Vector2(1.0f, 1.0f);

	public float MoveSpeed = 15;
	public float RotSpeed = 1;
	public float DodgeSpeed = 25;
	public float RayDist = 1.5f;
	public float RayRange = 12;

	public Vector2 DodgeDirection;

	private Rigidbody myRigidbody;

	public TrackPlayerState(GameObject self, Transform target)
	{
		this.target = target;
		this.Self = self;

		stateID = FSMStateID.TrackPlayer;

		myRigidbody = Self.GetComponent<Rigidbody>();
	}

	public override void EnterStateInit()
	{
		base.EnterStateInit();

		myRigidbody = Self.GetComponent<Rigidbody>();
	}

	public override void Act()
	{
		if (target == null) return;

		Quaternion oldRotation = Self.transform.rotation;

		// Rotate towards the target
		Quaternion rotation = Quaternion.LookRotation(target.transform.position - Self.transform.position);
		Self.transform.rotation = Quaternion.Slerp(Self.transform.rotation, rotation, Time.deltaTime * RotSpeed);

		// Look for obstacles
		EnemyUtilities.Dodge(Self.transform, ref DodgeDirection, ref ShouldDodge, DodgeSpeed, Size, RayDist, RayRange);

		if (ShouldDodge)
		{
			// Revert our rotations
			Self.transform.rotation = oldRotation;
			Vector3 newDodge = new Vector3(-DodgeDirection.y, DodgeDirection.x);
			Self.transform.Rotate(-newDodge * Time.deltaTime * DodgeSpeed);
		}

		//Self.transform.position += Self.transform.forward * MoveSpeed * Time.deltaTime;
	}

	public override void FixedAct()
	{
		// myRigidbody.AddRelativeForce(Vector3.forward, ForceMode.Acceleration)
		myRigidbody.MovePosition(myRigidbody.position + Self.transform.forward * MoveSpeed * Time.fixedDeltaTime);
	}

	public override void Reason()
	{
		Vector3 targetDir = target.position - Self.transform.position;
		float angle = Vector3.Angle(targetDir, Self.transform.forward);

		// Debug.Log((int)angle);
	}
}
