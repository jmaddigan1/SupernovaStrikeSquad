using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatingState : FSMState
{
	Transform retreatTarget;

	private EnemyBase enemy;
	private GameObject self;

	public RetreatingState(EnemyBase enemyBase)
	{
		stateID = FSMStateID.Retreating; 
		
		this.self = enemyBase.gameObject;
		this.enemy = enemyBase;
	}

	public override void EnterStateInit()
	{
		base.EnterStateInit();

		enemy.MoveSpeed = 35;
		enemy.RotationSpeed = 2.0f;
		enemy.RotationMultiplier = 1;

		retreatTarget = new GameObject("Retreat Target").transform;
		MoveRetreatPoint();

		enemy.Target = retreatTarget;
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

		retreatTarget.position = enemy.Target.position + new Vector3(x, y, z).normalized * dist;
	}

	public override void Reason()
	{
		if (EnemyUtilities.GetDistance(self.transform, enemy.Target.transform) < 10)
		{
			enemy.Target = null;
			FindTargets();
			GameObject.Destroy(retreatTarget.gameObject);
			enemy.PerformTransition(Transition.LostTarget);
		}
	}


	// Find all the players in the scene
	void FindTargets()
	{
		foreach (Target ship in GameObject.FindObjectsOfType<Target>())
		{
			// If we don't have a target, default this to the target
			if (enemy.Target == null)
			{
				enemy.Target = ship.transform;
				continue;
			}

			// Else we look for the closest player for our target
			if (Vector3.Distance(ship.transform.position, self.transform.position) <
				Vector3.Distance(enemy.Target.position, self.transform.position))
			{
				enemy.Target = ship.transform;
			}
		}
	}
}
