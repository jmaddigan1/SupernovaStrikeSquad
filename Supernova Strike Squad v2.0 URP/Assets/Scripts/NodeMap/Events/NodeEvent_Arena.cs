using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent_Arena : NodeEvent
{
	public NodeEvent_Arena()
	{
	}

	public override bool IsEventOver()
	{
		// Debug.Log("Event Update");

		return false;
	}

	public override void OnEventStart()
	{
		// ENVIRONMENT
		EnvironmentSpawner.Instance.Spawn(EnvironmentSpawner.Default());

		// ENEMYS
		EnemySpawner.Instance.Spawn(EnemySpawner.Default());
		
		// Move players to there starting positions
		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>())
		{
			// TODO: 
		}

		// Play Player Enter animation

		// Debug.Log("Event Start");
	}

	public override void OnEventEnd()
	{
		EnvironmentSpawner.Instance.Clear();

		EnemySpawner.Instance.Clear();

		// Debug.Log("Event End");
	}
}
