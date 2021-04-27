using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNSSPresets : MonoBehaviour
{
	#region Preset EnemyWaves

	public static EnemyWaveData DefaultAreana()
	{
		return new EnemyWaveData()
		{
			// The Waves in this Data
			EnemyWaves = new List<EnemyWave>()
			{
				// Wave ONE
				new EnemyWave()
				{
					// List of SpawnParameters for this wave
					EnemyList = new List<SpawnParameters>()
					{
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 3 },
						//new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 2 },
						//new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
					}
				},

				// Wave TWO
				new EnemyWave()
				{
					// List of SpawnParameters for this wave
					EnemyList = new List<SpawnParameters>()
					{
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
					}
				}
			}
		};
	}
	public static EnemyWaveData DefaultRun()
	{
		return new EnemyWaveData()
		{
			// The Waves in this Data
			EnemyWaves = new List<EnemyWave>()
			{
				// Wave ONE
				new EnemyWave()
				{
					// List of SpawnParameters for this wave
					EnemyList = new List<SpawnParameters>()
					{
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 15 },
						//new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 2 },
						//new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
					}
				},

				// Wave TWO
				new EnemyWave()
				{
					// List of SpawnParameters for this wave
					EnemyList = new List<SpawnParameters>()
					{
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
					}
				}
			}
		};
	}

	#endregion


	#region Preset Environments

	public static EnvironmentParameters DefaultAreanaEnvironment()
	{
		return new EnvironmentParameters()
		{
			EnvironmentType = EnvironmentType.Sphere,

			EnvironmentSize = new Vector3(200, 0, 0),

			AsteroidCount = 50,
			AsteroidMinSize = 25,
			AsteroidMaxSize = 150
		};
	}
	public static EnvironmentParameters DefaultRunEnvironment()
	{
		return new EnvironmentParameters()
		{
			EnvironmentType = EnvironmentType.Square,

			EnvironmentSize = new Vector3(200, 200, 2000),

			AsteroidCount = 150,
			AsteroidMinSize = 35,
			AsteroidMaxSize = 200
		};
	}
	public static EnvironmentParameters DefaultBossEnvironment()
	{
		return new EnvironmentParameters()
		{
			EnvironmentType = EnvironmentType.Sphere,

			EnvironmentSize = new Vector3(500, 0, 0),

			AsteroidCount = 75,
			AsteroidMinSize = 35,
			AsteroidMaxSize = 200
		};
	}

	#endregion


	#region Preset Events

	public static NodeEvent TestAreana()
	{
		NodeEvent nodeEvent = new NodeEvent_Arena()
		{
			EventName = "Test Areana",

			EventDescription = "A description for the test Areana"
		};

		return nodeEvent;
	}
	public static NodeEvent TestRunner()
	{
		NodeEvent nodeEvent = new NodeEvent_Runner()
		{
			EventName = "Test Runner",

			EventDescription = "A description for the test Runner"
		};

		return nodeEvent;
	}
	public static NodeEvent TestBoss()
	{
		NodeEvent nodeEvent = new NodeEvent_Boss()
		{
			EventName = "Test Boss",

			EventDescription = "A description for the test Boss"
		};

		return nodeEvent;
	}

	#endregion
}
