using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawner : NetworkBehaviour
{
	// Static Members
	// The Enemy Spawner Singleton
	public static EnemySpawner Instance;


	// Editor References
	// A list of enemies we want to register and  add to the enemy dictionary
	[SerializeField]
	private List<Enemy> EnemyPrefabs = new List<Enemy>();


	// Private Members
	// This Dictionary is used to link enemy prefabs with Enemy IDs
	private Dictionary<EnemyType, Enemy> enemyDictionary = new Dictionary<EnemyType, Enemy>();

	void Start()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}

		Instance = this;

		BuildDictionary();
	}

	//
	public void BuildDictionary()
	{
		foreach (Enemy enemy in EnemyPrefabs)
		{
			if (!enemyDictionary.ContainsKey(enemy.ID))
			{
				enemyDictionary.Add(enemy.ID, enemy);
			}
			else
			{
				Debug.LogError("This Enemy ID is already registered");
			}
		}
	}

	[Server]
	public void SpawnWave(EnemyWave enemyWave, Action<Enemy> onDeathCallback = null)
	{
		foreach (SpawnParameters enemy in enemyWave.Enemies) SpawnEnemies(enemy, onDeathCallback);
	}

	[Server]
	void SpawnEnemies(SpawnParameters spawnParameters, Action<Enemy> onDeathCallback = null)
	{
		for (int count = 0; count < spawnParameters.EnemyCount; count++)
		{
			SpawnEnemy(spawnParameters.EnemyID, onDeathCallback);
		}
	}

	[Server]
	private Enemy SpawnEnemy(EnemyType enemy, Action<Enemy> onDeathCallback = null)
	{
		if (enemyDictionary.ContainsKey(enemy) == false)
		{
			Debug.LogError("This enemy is not registered!");
		}
		else
		{
			Enemy newEnemy = Instantiate(enemyDictionary[enemy]);
			newEnemy.OnDeath += onDeathCallback;

			NetworkServer.Spawn(newEnemy.gameObject);

			return newEnemy;
		}

		return null;
	}

	#region Static Singleton Wrappers

	public static void SpawnEnemies(EnemyWave enemyWave, Action<Enemy> onDeathCallback = null)	{
		if (ValidSingleton()) Instance.SpawnWave(enemyWave, onDeathCallback); 
	}

	static bool ValidSingleton()
	{
		if (Instance == null)
		{
			// Debug.LogError("ERROR: There is no 'EnemySpawner' in the scene");
			return false;
		}

		return true;
	}

	#endregion
}
