using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
	public static NodeEvent ArenaWaves()
	{
		NodeEvent_Arean nodeEvent = new NodeEvent_Arean();

		nodeEvent.Name = "Arena Waves";

		nodeEvent.WaveData =
			new EnemyWaveData
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

		nodeEvent.Environment =
			new EnvironmentData
			{
				Size = 200,

				AsteroidCount = 40,
				MinAsteroidSize = 5,
				MaxAsteroidSize = 55,
			};

		return nodeEvent;
	}
	public static NodeEvent LargeArena()
	{
		NodeEvent_Arean nodeEvent = new NodeEvent_Arean();

		nodeEvent.Name = "Large Arena";

		nodeEvent.WaveData =
			new EnemyWaveData
			{
				Waves = new List<EnemyWave>
			{
				new EnemyWave {
					Enemies = new List<SpawnParameters> {
						new SpawnParameters { EnemyCount = 15, EnemyID = EnemyID.ExampleEnemyID1 },
						new SpawnParameters { EnemyCount = 10, EnemyID = EnemyID.ExampleEnemyID2 },
						new SpawnParameters { EnemyCount = 5, EnemyID = EnemyID.ExampleEnemyID3 },
					}
				},
			}
			};

		nodeEvent.Environment =
			new EnvironmentData
			{
				Size = 750,

				AsteroidCount = 100,
				MinAsteroidSize = 25,
				MaxAsteroidSize = 300,
			};

		return nodeEvent;
	}
	public static NodeEvent ClusterArena()
	{
		NodeEvent_Arean nodeEvent = new NodeEvent_Arean();

		nodeEvent.Name = "Cluster Arena";

		nodeEvent.WaveData =
			new EnemyWaveData
			{
				Waves = new List<EnemyWave>
			{
				new EnemyWave {
					Enemies = new List<SpawnParameters> {
						new SpawnParameters { EnemyCount = 10, EnemyID = EnemyID.ExampleEnemyID1 },
					}
				},
			}
			};

		nodeEvent.Environment =
			new EnvironmentData
			{
				Size = 750,

				AsteroidCount = 500,
				MinAsteroidSize = 50,
				MaxAsteroidSize = 250,
			};

		return nodeEvent;
	}
}
