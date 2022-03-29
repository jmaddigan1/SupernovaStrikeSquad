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

public class TrackPlayerStateData
{
	public float MoveSpeed = 25f;
	public float RotSpeed = 1;
	public float RotationMultiplier = 1f;
	public float DodgeSpeed = 25;
	public float RayDist = 1.5f;
	public float RayRange = 12;
	public bool ShouldDodge = false;
	public Vector2 Size = new Vector2(1.0f, 1.0f);
}

public class TrackPlayerState : FSMState
{
	private EnemyStateData enemyData;

	private Rigidbody myRigidbody;

	public Vector2 dodgeDirection;

	// Properties
	public GameObject Self { get { return enemyData.EnemyBase.gameObject; } }

	public TrackPlayerState(EnemyStateData enemyData )
	{
		stateID = FSMStateID.TrackTarget;

		this.enemyData = enemyData;

		EnemyUtilities.FindTarget(enemyData.Movement);

		myRigidbody = Self.GetComponent<Rigidbody>();
	}

	public override void EnterStateInit()
	{
		enemyData.Movement.MoveSpeed = 25;
		enemyData.Movement.RotationSpeed = 2;
		enemyData.Movement.RotationMultiplier = 1;

		EnemyUtilities.FindTarget(enemyData.Movement);
	}

	public override void Act()
	{
	}

	public override void Reason()
	{

		// If the Player is in attack range
		if (enemyData.Movement.Target == null) return;


		float targetAngle = EnemyUtilities.GetAngle(Self.transform, enemyData.Movement.Target);
		if (targetAngle < enemyData.Movement.AttackAngle)
		{
			//Debug.Log($"TrackPlayerState | Found Player!");
			enemyData.Movement.PerformTransition(Transition.FoundTarget);
			return;
		}

		// If the player is too far away	
		float targetDist = EnemyUtilities.GetDistance(Self.transform, enemyData.Movement.Target);
		if (targetDist > enemyData.Movement.EscapeRange)
		{
			//Debug.Log($"TrackPlayerState | Lost Player!");
			enemyData.Movement.PerformTransition(Transition.LostTarget);
			return;
		}

		if (targetDist < 10)
		{
			//Debug.Log($"TrackPlayerState | Lost Player!");
			enemyData.Movement.PerformTransition(Transition.ApproachedPlayer);
			return;
		}
	}
}
