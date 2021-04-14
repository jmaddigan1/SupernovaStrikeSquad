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
	private GameObject self;
	private List<Target> ships = new List<Target>();

	private Timer shootTimer;

	public ShootPlayerState(EnemyBase enemyBase, ShootPlayerStateData data)
	{
		stateID = FSMStateID.ShootPlayer;

		this.self = enemyBase.gameObject;
		this.enemy = enemyBase;
		this.data = data;

		if (enemy.shot != null) {
			shootTimer = new Timer(0.1f, Shoot);
		}

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
		if (EnemyUtilities.GetAngle(self.transform, enemy.Target) < 10)
		{
			shootTimer?.IncrementTime();
		}
	}

	public override void Reason()
	{
		if (enemy.Target == null) return;

		// If the Player has exited out attack range
		if (EnemyUtilities.GetAngle(self.transform, enemy.Target) > 25)
		{
			//Debug.Log($"ShootState | Lost Player!");
			enemy.PerformTransition(Transition.LostTarget);
			return;
		}

		if (EnemyUtilities.GetDistance(self.transform, enemy.Target) < 35)
		{
			//Debug.Log($"Done Shooting | Done Shooting!");
			enemy.PerformTransition(Transition.ApproachedPlayer);
			return;
		}
	}

	public void Shoot()
	{
		enemy.Shoot(enemy.Target);
	}

	void FindTargets()
	{

		Debug.Log("Look for Targets: " + GameObject.FindObjectsOfType<ShipController>().Length);

		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>())
		{
			if (enemy.Target == null)
			{
				enemy.Target = ship.transform;
				continue;
			}

			if (Vector3.Distance(ship.transform.position, self.transform.position) <
				Vector3.Distance(enemy.Target.position, self.transform.position))
			{
				//Debug.Log("Found Target");
				enemy.Target = ship.transform;
			}
		}
	}
}
