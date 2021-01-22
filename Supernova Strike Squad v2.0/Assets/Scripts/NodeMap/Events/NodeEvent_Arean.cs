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
	public EnemyWaveData currentWaveData;

	public override void OnStartEvent()
	{
		base.OnStartEvent();

		EnemyCount = 0;
		WaveIndex = 0;

		EnemyWaveData waveData = new EnemyWaveData
		{
			Waves = new List<EnemyWave>
			{
				new EnemyWave {
					Enemies = new List<SpawnParameters> {
						new SpawnParameters { EnemyCount = 3, EnemyID = EnemyID.ExampleEnemyID1 },
						new SpawnParameters { EnemyCount = 2, EnemyID = EnemyID.ExampleEnemyID2 },
					}
				},
				new EnemyWave {
					Enemies = new List<SpawnParameters> {
						new SpawnParameters { EnemyCount = 5, EnemyID = EnemyID.ExampleEnemyID1 },
						new SpawnParameters { EnemyCount = 2, EnemyID = EnemyID.ExampleEnemyID2 },
						new SpawnParameters { EnemyCount = 1, EnemyID = EnemyID.ExampleEnemyID3 },
					}
				}
			}
		};

		Environment = new EnvironmentData
		{
			Size = 100,

			AsteroidCount = 50,
			MinAsteroidSize = 5,
			MaxAsteroidSize = 55,
		};

		currentWaveData = waveData;

		EnemyCount = currentWaveData.Waves[WaveIndex].GetEnemyCount();

		EnemySpawner.SpawnEnemies(currentWaveData.Waves[WaveIndex], OnEnemyDeath);
	}

	void OnEnemyDeath(Enemy enemy)
	{
		EnemyCount--;

		if (EnemyCount <= 0)
		{
			WaveIndex++;

			if (WaveIndex > currentWaveData.Waves.Count - 1)
			{
				Debug.Log("Event Done");
			}
			else
			{
				EnemyCount = currentWaveData.Waves[WaveIndex].GetEnemyCount();

				EnemySpawner.SpawnEnemies(currentWaveData.Waves[WaveIndex], OnEnemyDeath);
			}
		}
	}

	public override bool IsOver()
	{
		return EnemyCount <= 0;
	}
}
