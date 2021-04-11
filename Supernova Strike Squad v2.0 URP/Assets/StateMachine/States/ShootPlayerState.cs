using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayerStateData
{
	public float MoveSpeed = 5f;
	public float RotSpeed = 0.4f;
	public float RotationMultiplier = 1f;
	public float DodgeSpeed = 25;
	public float RayDist = 1.5f;
	public float RayRange = 12;
	public bool ShouldDodge = false;
	public Vector2 Size = new Vector2(1.0f, 1.0f);
}

public class ShootPlayerState : FSMState
{
	private ShootPlayerStateData data;
	private EnemyBase enemy;
	private Transform target;
	private GameObject self;
	private List<Target> ships = new List<Target>();

	private Timer shootTimer;

	public ShootPlayerState(EnemyBase enemyBase, ShootPlayerStateData data)
	{
		stateID = FSMStateID.ShootPlayer;

		this.self = enemyBase.gameObject;
		this.enemy = enemyBase;
		this.data = data;

		shootTimer = new Timer(0.1f, Shoot);

		FindTargets();
	}

	public override void EnterStateInit()
	{
		enemy.MoveSpeed = 15;
		enemy.RotationSpeed = 1.5f;
		enemy.RotationMultiplier = 1;
	}

	public override void Act()
	{
		// If the Player has exited out attack range
		if (EnemyUtilities.GetAngle(self.transform, target) < 10)
		{
			shootTimer.IncrementTime();
		}
	}

	public override void Reason()
	{
		if (target == null) return;

		// If the Player has exited out attack range
		if (EnemyUtilities.GetAngle(self.transform, target) > 25)
		{
			Debug.Log($"ShootState | Lost Player!");
			enemy.PerformTransition(Transition.LostTarget);
			return;
		}

		if (EnemyUtilities.GetDistance(self.transform, target) < 35)
		{
			Debug.Log($"Done Shooting | Done Shooting!");
			enemy.PerformTransition(Transition.ApproachedPlayer);
			return;
		}
	}

	public void Shoot()
	{
		enemy.Shoot();
	}

	void FindTargets()
	{
		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>())
		{
			if (target == null)
			{
				target = ship.transform;
				continue;
			}

			if (Vector3.Distance(ship.transform.position, self.transform.position) <
				Vector3.Distance(target.position, self.transform.position))
			{
				Debug.Log("Found Target");
				target = ship.transform;
			}
		}
	}
}
