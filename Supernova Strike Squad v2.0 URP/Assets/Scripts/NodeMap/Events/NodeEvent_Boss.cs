using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent_Boss : NodeEvent
{
	public override bool IsEventOver()
	{
		return false;
	}

	public override void OnEventStart()
	{
		// ENVIRONMENT
		Environment = EnvironmentSpawner.DefaultBoss();
		EnvironmentSpawner.Instance.Spawn(Environment);

		// ENEMYS
		EnemySpawner.Instance.SpawnEnemy(EnemyType.TestBoss, OnBossDeath);

		// Move players to there starting positions
		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>()) {
			ship.transform.position = -Vector3.forward * Environment.EnvironmentSize.x / 2;
		}

		// Play Player Enter animation

		// Debug.Log("Event Start");
	}

	public override void OnEventEnd()
	{
	}

	public void OnBossDeath()
	{

	}
}
