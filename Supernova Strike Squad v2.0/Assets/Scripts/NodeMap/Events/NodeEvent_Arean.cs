using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent_Arean : NodeEvent
{
	// public Members
	// The amount of enemies that are currently in the game
	public int EnemyCount;

	// The waveIndex we are on
	public int WaveIndex;

	// The Wave data we are managing
	public EnemyWaveData WaveData;

	public override void OnStartEvent()
	{
		base.OnStartEvent();

		EnemyCount = 0;
		WaveIndex = 0;

		EnemyCount = WaveData.Waves[WaveIndex].GetEnemyCount();

		EnemySpawner.SpawnEnemies(WaveData.Waves[WaveIndex], OnEnemyDeath);
	}

	void OnEnemyDeath(Enemy enemy)
	{
		EnemyCount--;

		if (EnemyCount <= 0)
		{
			WaveIndex++;

			if (WaveIndex > WaveData.Waves.Count - 1)
			{
				Debug.Log("Event Done");
			}
			else
			{
				EnemyCount = WaveData.Waves[WaveIndex].GetEnemyCount();

				EnemySpawner.SpawnEnemies(WaveData.Waves[WaveIndex], OnEnemyDeath);
			}
		}
	}

	public override bool IsOver()
	{
		return EnemyCount <= 0;
	}
}
