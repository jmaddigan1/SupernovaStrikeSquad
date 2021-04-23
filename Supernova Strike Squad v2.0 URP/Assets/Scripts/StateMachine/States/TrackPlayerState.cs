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
	// Private Members
	private TrackPlayerStateData data;
	private Rigidbody myRigidbody;
	private EnemyBase enemy;
	private GameObject self;

	public Vector2 dodgeDirection;

	public TrackPlayerState(EnemyBase enemyBase, TrackPlayerStateData data)
	{
		stateID = FSMStateID.TrackTarget;

		//this.target = enemyBase.Target;
		this.self = enemyBase.gameObject;
		this.enemy = enemyBase;
		this.data = data;

		EnemyUtilities.FindTarget(enemy);

		myRigidbody = self.GetComponent<Rigidbody>();
	}

	public override void EnterStateInit()
	{
		enemy.MoveSpeed = 25;
		enemy.RotationSpeed = 2;
		enemy.RotationMultiplier = 1;

		EnemyUtilities.FindTarget(enemy);
	}

	public override void Act()
	{
	}

	public override void Reason()
	{

		// If the Player is in attack range
		if (enemy.Target == null) return;


		float targetAngle = EnemyUtilities.GetAngle(self.transform, enemy.Target);
		if (targetAngle < enemy.AttackAngle)
		{
			//Debug.Log($"TrackPlayerState | Found Player!");
			enemy.PerformTransition(Transition.FoundTarget);
			return;
		}

		// If the player is too far away	
		float targetDist = EnemyUtilities.GetDistance(self.transform, enemy.Target);
		if (targetDist > enemy.EscapeRange)
		{
			//Debug.Log($"TrackPlayerState | Lost Player!");
			enemy.PerformTransition(Transition.LostTarget);
			return;
		}

		if (targetDist < 10)
		{
			//Debug.Log($"TrackPlayerState | Lost Player!");
			enemy.PerformTransition(Transition.ApproachedPlayer);
			return;
		}
	}
}
