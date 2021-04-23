using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
	private List<Vector3> patrolPoints;

	private EnemyBase enemy;
	private GameObject self;

	private List<Target> ships = new List<Target>();

	private Timer findPlayersTimer;
	private Timer updatePatrolPointTimer;

	private int pointIndex;

	public PatrolState(EnemyBase enemyBase, List<Vector3> patrolPoints)
	{
		stateID = FSMStateID.Patrol;

		this.patrolPoints = patrolPoints;
		this.self = enemyBase.gameObject;
		this.enemy = enemyBase;

		enemy.Target = null;

		findPlayersTimer = new Timer(2f, FindTargets);
		updatePatrolPointTimer = new Timer(5f, UpdatePatrolPoint);

	}

	public override void EnterStateInit()
	{
		enemy.MoveSpeed = 25;
		enemy.RotationSpeed = 1;
		enemy.RotationMultiplier = 1;
		enemy.Target = null;
	}

	public override void Act()
	{
		enemy.Target = null;

		// Check if we are near the patrol point.
		if (EnemyUtilities.GetDistance(self.transform, patrolPoints[pointIndex]) < enemy.PatrolPointRange)
		{
			pointIndex = (pointIndex + 1) % patrolPoints.Count;
		}

		// Look at the target
		float rotSpeed = enemy.RotationSpeed * enemy.RotationMultiplier;
		Quaternion neededRotation = Quaternion.LookRotation(patrolPoints[pointIndex] - self.transform.position, self.transform.up);
		self.transform.rotation = Quaternion.Slerp(self.transform.rotation, neededRotation, Time.deltaTime * rotSpeed);

		// If the target is NOT in front of us
		if (EnemyUtilities.GetAngle(self.transform, patrolPoints[pointIndex]) > 1)
		{
			// We want to slowly increase out rotation speed multiplier
			enemy.RotationMultiplier = Mathf.Lerp(enemy.RotationMultiplier, 3f, Time.deltaTime * 0.5f);
		}
		else
		{
			// Else we are looking at the target
			enemy.RotationMultiplier = Mathf.Lerp(enemy.RotationMultiplier, 1f, Time.deltaTime * 3.5f);
		}

		// Move forward
		self.transform.position += self.transform.forward * enemy.MoveSpeed * Time.deltaTime;

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

		if (self.transform == null) return;
		

		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>())
		{
			// If we don't have a target, default this to the target
			if (currentTarget == null)
			{
				currentTarget = ship.transform;
				continue;
			}

			Debug.Log(ship.transform);
			Debug.Log(self.transform);

			// Else we look for the closest player for our target
			if (Vector3.Distance(ship.transform.position, self.transform.position) <
				Vector3.Distance(currentTarget.position, self.transform.position))
			{
				currentTarget = ship.transform;
				enemy.Target = ship.transform;
			}
		}

		if (currentTarget == null) return;

		if (Vector3.Distance(self.transform.position, currentTarget.position) < enemy.PatrolDetectionRange)
		{
			// Debug.Log($"PatrolState | Found a Player!");
			enemy.PerformTransition(Transition.FoundTarget);
		}
	}

	// Increment the patrol point
	void UpdatePatrolPoint()
	{
		pointIndex = (pointIndex + 1) % patrolPoints.Count;
	}
}
