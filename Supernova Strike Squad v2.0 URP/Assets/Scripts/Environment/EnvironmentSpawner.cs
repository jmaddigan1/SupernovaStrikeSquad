using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnvironmentParameters
{
	public float EnvironmentSize;

	public int AsteroidCount;
	public float AsteroidMinSize;
	public float AsteroidMaxSize;
}

public class EnvironmentSpawner : NetworkBehaviour
{
	public static EnvironmentSpawner Instance;

	[SerializeField]
	private GameObject AsteroidPrefab = null;


	public EnvironmentParameters CurrentEnvironment;

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
	public void Spawn(EnvironmentParameters environment)
	{
		CurrentEnvironment = environment;

		for (int count = 0; count < environment.AsteroidCount; count++)
		{
			// POSITION
			Vector3 point = GetRandomPosition(environment.EnvironmentSize);

			// SCALE
			float size = GetRandomSize(environment.AsteroidMinSize, environment.AsteroidMaxSize);

			// SPAWN
			GameObject go = Instantiate(AsteroidPrefab, point, Quaternion.identity, transform);
			go.transform.localScale = Vector3.one * size;
			NetworkServer.Spawn(go);
		}
	}

	[Server]
	public void Clear()
	{

	}

	public Vector3 GetRandomPosition(float range)
	{
		Vector3 point = Vector3.zero;

		point.x = Random.Range(-1f, 1);
		point.y = Random.Range(-1f, 1);
		point.z = Random.Range(-1f, 1);

		return point.normalized * range;
	}

	public float GetRandomSize(float min, float max)
	{
		return Random.Range(min, max);
	}

	void OnDrawGizmos()
	{
		if (CurrentEnvironment == null) return;

		Gizmos.DrawWireSphere(transform.position, CurrentEnvironment.EnvironmentSize);
	}

	public static EnvironmentParameters Default()
	{
		return new EnvironmentParameters()
		{
			EnvironmentSize = 200f,

			AsteroidCount = 50,
			AsteroidMinSize = 15,
			AsteroidMaxSize = 35
		};
	}
}
