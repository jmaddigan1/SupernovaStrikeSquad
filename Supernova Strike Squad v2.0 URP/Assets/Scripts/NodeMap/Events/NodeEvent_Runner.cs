using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent_Runner : NodeEvent
{
	private ShipController[] ships;

	public override bool IsEventOver()
	{
		foreach (ShipController ship in ships)
		{
			Vector3 endPoint = new Vector3(0, 0, Environment.EnvironmentSize.z);

			if (Vector3.Distance(ship.transform.position, endPoint) < 100) {
				return true;
			}
		}

		return false;
	}

	public override void OnEventStart()
	{
		// ENVIRONMENT
		Environment = EnvironmentSpawner.DefaultRun();
		EnvironmentSpawner.Instance.Spawn(Environment);

		// ENEMYS
		// EnemySpawner.Instance.Spawn(EnemySpawner.Default());

		// Get players
		ships = GameObject.FindObjectsOfType<ShipController>();

		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>()) {
			ship.transform.position = -Vector3.forward * Environment.EnvironmentSize.z;
		}
	}

	public override void OnEventEnd()
	{
		EnvironmentSpawner.Instance.Clear();

		EnemySpawner.Instance.Clear();
	}
}
