using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatingState : FSMState
{
	private EnemyStateData enemyData;

	Transform retreatTarget;

	// Properties
	public GameObject Self { get { return enemyData.EnemyBase.gameObject; } }

	public RetreatingState(EnemyStateData enemyData)
	{
		stateID = FSMStateID.Retreating;

		this.enemyData = enemyData;
	}

	public override void EnterStateInit()
	{
		base.EnterStateInit();

		enemyData.Movement.MoveSpeed = 35;
		enemyData.Movement.RotationSpeed = 2;
		enemyData.Movement.RotationMultiplier = 1;

		retreatTarget = new GameObject("Retreat Target").transform;

		MoveRetreatPoint();

		enemyData.Movement.Target = retreatTarget;
	}

	public override void Act()
	{
	}

	void MoveRetreatPoint()
	{
		float x = Random.Range(-1, 1f);
		float y = Random.Range(-1, 1f);
		float z = Random.Range(-1, 1f);

		float dist = Random.Range(25f, 50f);

		retreatTarget.position = enemyData.Movement.Target.position + new Vector3(x, y, z).normalized * dist;
	}

	public override void Reason()
	{
		if (EnemyUtilities.GetDistance(Self.transform, enemyData.Movement.Target.transform) < 10)
		{
			enemyData.Movement.Target = null;
			FindTargets();
			GameObject.Destroy(retreatTarget.gameObject);
			enemyData.Movement.PerformTransition(Transition.LostTarget);
		}
	}


	// Find all the players in the scene
	void FindTargets()
	{
		foreach (Target ship in GameObject.FindObjectsOfType<Target>())
		{
			// If we don't have a target, default this to the target
			if (enemyData.Movement.Target == null)
			{
				enemyData.Movement.Target = ship.transform;
				continue;
			}

			// Else we look for the closest player for our target
			if (Vector3.Distance(ship.transform.position, Self.transform.position) <
				Vector3.Distance(enemyData.Movement.Target.position, Self.transform.position))
			{
				enemyData.Movement.Target = ship.transform;
			}
		}
	}
}
