using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum EnemyType
{
	TestEnemy
}

public class EnemySpawner : NetworkBehaviour
{
	public static EnemySpawner Instance;

	[SerializeField]
	private List<EnemyBase> enemyPrefabs = new List<EnemyBase>();

	public EnemyWaveData CurrentEncounter;

	int currentWave;


	Dictionary<EnemyType, GameObject> EnemyDictionary = new Dictionary<EnemyType, GameObject>();

	public override void OnStartClient()
	{
		foreach (EnemyBase enemy in enemyPrefabs)
		{
			if (EnemyDictionary.ContainsKey(enemy.EnemyType))
			{
				Debug.Log($"This Enemy is already Registered: {enemy.name}");
			}
			else
			{
				EnemyDictionary.Add(enemy.EnemyType, enemy.gameObject);
				NetworkClient.RegisterPrefab(enemy.gameObject);
			}
		}
	}

	public override void OnStartServer()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	[Server]
	public void Clear()
	{

	}

	[Server]
	public void Spawn(EnemyWaveData encounterData)
	{
		// Clear the last encounter
		Clear();

		// Update the current encounter
		CurrentEncounter = encounterData;
		currentWave = 0;

		SpawnWave(CurrentEncounter.EnemyWaves[currentWave]);
	}

	void SpawnWave(EnemyWave enemyWaveData)
	{
		foreach (SpawnParameters enemy in enemyWaveData.EnemyList)
		{
			SpawnEnemy(enemy);
		}
	}

	void SpawnEnemy(SpawnParameters enemyData)
	{
		for (int count = 0; count < enemyData.EnemyCount; count++)
		{
			SpawnEnemy(enemyData.Enemy);
		}
	}

	void SpawnEnemy(EnemyType enemy, System.Action OnDeathCallback = null)
	{
		GameObject go = Instantiate(EnemyDictionary[enemy]);
		NetworkServer.Spawn(go);
	}

	public static EnemyWaveData Default()
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
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
						new SpawnParameters(){ Enemy = EnemyType.TestEnemy,EnemyCount = 1 },
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
}