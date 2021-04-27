using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
	private EnemyStateData enemyData;

	private List<Vector3> patrolPoints;

	private List<Target> targets = new List<Target>();

	private Timer findPlayersTimer;
	private Timer updatePatrolPointTimer;

	private int pointIndex;

	// Properties
	public GameObject Self { get { return enemyData.EnemyBase.gameObject; } }

	// Constructor
	public PatrolState(EnemyStateData enemyData, List<Vector3> patrolPoints)
	{
		stateID = FSMStateID.Patrol;

		this.patrolPoints = patrolPoints;
		this.enemyData = enemyData;

		findPlayersTimer = new Timer(2f, FindTargets);
		updatePatrolPointTimer = new Timer(5f, UpdatePatrolPoint);

		enemyData.Movement.Target = null;
	}

	public override void EnterStateInit()
	{
		enemyData.Movement.MoveSpeed = 25;
		enemyData.Movement.RotationSpeed = 1;
		enemyData.Movement.RotationMultiplier = 1;

		enemyData.Movement.Target = null;
	}

	public override void Act()
	{
		enemyData.Movement.Target = null;

		// Check if we are near the patrol point.
		if (EnemyUtilities.GetDistance(Self.transform, patrolPoints[pointIndex]) < enemyData.Movement.PatrolPointRange)
		{
			pointIndex = (pointIndex + 1) % patrolPoints.Count;
		}

		// Look at the target
		float rotSpeed = enemyData.Movement.RotationSpeed * enemyData.Movement.RotationMultiplier;
		Quaternion neededRotation = Quaternion.LookRotation(patrolPoints[pointIndex] - Self.transform.position, Self.transform.up);
		Self.transform.rotation = Quaternion.Slerp(Self.transform.rotation, neededRotation, Time.deltaTime * rotSpeed);

		// If the target is NOT in front of us
		if (EnemyUtilities.GetAngle(Self.transform, patrolPoints[pointIndex]) > 1)
		{
			// We want to slowly increase out rotation speed multiplier
			enemyData.Movement.RotationMultiplier = Mathf.Lerp(enemyData.Movement.RotationMultiplier, 3f, Time.deltaTime * 0.5f);
		}
		else
		{
			// Else we are looking at the target
			enemyData.Movement.RotationMultiplier = Mathf.Lerp(enemyData.Movement.RotationMultiplier, 1f, Time.deltaTime * 3.5f);
		}

		// Move forward
		Self.transform.position += Self.transform.forward * enemyData.Movement.MoveSpeed * Time.deltaTime;

		// Update the Timers
		updatePatrolPointTimer.IncrementTime();
		findPlayersTimer.IncrementTime();
	}

	public override void Reason()
	{
	}

	// Find all the players in the scene
	void FindTargets()
	{
		Transform currentTarget = null;

		if (Self.transform == null) return;

		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>())
		{
			// If we don't have a target, default this to the target
			if (currentTarget == null)
			{
				currentTarget = ship.transform;
				continue;
			}

			Debug.Log(ship.transform);
			Debug.Log(Self.transform);

			// Else we look for the closest player for our target
			if (Vector3.Distance(ship.transform.position, Self.transform.position) <
				Vector3.Distance(currentTarget.position, Self.transform.position))
			{
				currentTarget = ship.transform;
				enemyData.Movement.Target = ship.transform;
			}
		}

		if (currentTarget == null) return;

		if (Vector3.Distance(Self.transform.position, currentTarget.position) < enemyData.Movement.PatrolDetectionRange)
		{
			// Debug.Log($"PatrolState | Found a Player!");
			enemyData.Movement.PerformTransition(Transition.FoundTarget);
		}
	}

	// Increment the patrol point
	void UpdatePatrolPoint()
	{
		pointIndex = (pointIndex + 1) % patrolPoints.Count;
	}
}
