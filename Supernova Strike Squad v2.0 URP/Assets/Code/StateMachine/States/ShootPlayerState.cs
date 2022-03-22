using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayerState : FSMState
{
	private EnemyStateData enemyData;

	private Timer shootTimer;

	private List<Target> targets = new List<Target>();

	// Properties
	public GameObject Self { get { return enemyData.EnemyBase.gameObject; } }

	public ShootPlayerState(EnemyStateData enemyData)
	{
		stateID = FSMStateID.ShootPlayer;

		this.enemyData = enemyData;

		shootTimer = new Timer(0.1f, Shoot);

		EnemyUtilities.FindTarget(enemyData.Movement);
	}

	public override void EnterStateInit()
	{
		enemyData.Movement.MoveSpeed = 15;
		enemyData.Movement.RotationSpeed = 1.5f;
		enemyData.Movement.RotationMultiplier = 1.0f;
	}

	public override void Act()
	{
		if (enemyData.Movement.Target == null) return;

		// If the Player has exited out attack range
		if (EnemyUtilities.GetAngle(Self.transform, enemyData.Movement.Target) < 10)
		{
			shootTimer?.IncrementTime();
		}
	}

	public override void Reason()
	{
		if (enemyData.Movement.Target == null) return;

		// If the Player has exited out attack range
		if (EnemyUtilities.GetAngle(Self.transform, enemyData.Movement.Target) > 15)
		{
			//Debug.Log($"ShootState | Lost Player!");
			enemyData.Movement.PerformTransition(Transition.LostTarget);
			return;
		}

		if (EnemyUtilities.GetDistance(Self.transform, enemyData.Movement.Target) < 35)
		{
			//Debug.Log($"Done Shooting | Done Shooting!");
			enemyData.Movement.PerformTransition(Transition.ApproachedPlayer);
			return;
		}

		if (EnemyUtilities.GetDistance(Self.transform, enemyData.Movement.Target) > enemyData.Movement.EscapeRange)
		{
			//Debug.Log($"ShootState | Lost Player!");
			enemyData.Movement.PerformTransition(Transition.LostTarget);
			return;
		}
	}

	public void Shoot()
	{
		enemyData.EnemyBase.Shoot(enemyData.Movement.Target);
	}
}
