using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent_Arena : NodeEvent
{
	bool eventOver = false;

	public NodeEvent_Arena()
	{
	}

	public override bool IsEventOver()
	{
		return eventOver;
	}

	public override void OnEventStart()
	{
		// ENVIRONMENT
		Environment = SNSSPresets.DefaultAreanaEnvironment();
		EnvironmentSpawner.Instance.Spawn(Environment);

		// ENEMYS
		EnemySpawner.Instance.Spawn(SNSSPresets.DefaultAreana(), OnEndEncounter);
		
		// Move players to there starting positions
		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>())
		{
			ship.transform.position = -Vector3.forward * Environment.EnvironmentSize.x / 2;
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

	public void OnEndEncounter(bool victory)
	{
		eventOver = true;
	}
}
