using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum EnvironmentType
{
	Sphere, 
	Square
}

public class EnvironmentParameters
{
	public EnvironmentType EnvironmentType;

	public Vector3 EnvironmentSize;

	public int AsteroidCount;
	public float AsteroidMinSize;
	public float AsteroidMaxSize;
}

public class EnvironmentSpawner : NetworkBehaviour
{
	public static EnvironmentSpawner Instance;

	[SerializeField]
	private GameObject AsteroidPrefab = null;

	[SerializeField]
	private AnimationCurve spawnCurve;

	[SerializeField]
	private AnimationCurve sizeCurve;

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
			Vector3 point = GetRandomPosition(environment.EnvironmentSize, environment.EnvironmentType);

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

	public Vector3 GetRandomPosition(Vector3 environmentSize, EnvironmentType environmentType)
	{
		Vector3 point = Vector3.zero;

		if (environmentType == EnvironmentType.Sphere)
		{
			point.x = Random.Range(-1f, 1);
			point.y = Random.Range(-1f, 1);
			point.z = Random.Range(-1f, 1);

			return point.normalized * spawnCurve.Evaluate(Random.Range(0,1f)) * environmentSize.x;
		}

		if (environmentType == EnvironmentType.Square)
		{
			point.x = Random.Range(-1f, 1) * (environmentSize.x);
			point.y = Random.Range(-1f, 1) * (environmentSize.y);
			point.z = Random.Range(-1f, 1) * (environmentSize.z);

			point.z += 50;

			return point;
		}

		return point;
	}

	public float GetRandomSize(float min, float max)
	{
		return (Random.Range(min, max) * sizeCurve.Evaluate(Random.Range(0f, 1f)));
	}

	void OnDrawGizmos()
	{
		if (CurrentEnvironment == null) return;

		// SPHERE
		if (CurrentEnvironment.EnvironmentType == EnvironmentType.Sphere)
		{
			Gizmos.DrawWireSphere(transform.position, CurrentEnvironment.EnvironmentSize.x);
		}

		// SQUARE
		if (CurrentEnvironment.EnvironmentType == EnvironmentType.Square)
		{
			Gizmos.DrawWireCube(transform.position, CurrentEnvironment.EnvironmentSize * 2);
		}
	}

	public static EnvironmentParameters DefaultAreana()
	{
		return new EnvironmentParameters()
		{
			EnvironmentType = EnvironmentType.Sphere,

			EnvironmentSize = new Vector3(200,0,0),

			AsteroidCount = 50,
			AsteroidMinSize = 50,
			AsteroidMaxSize = 150
		};
	}
	public static EnvironmentParameters DefaultRun()
	{
		return new EnvironmentParameters()
		{
			EnvironmentType = EnvironmentType.Square,

			EnvironmentSize = new Vector3(200, 200, 2000),

			AsteroidCount = 75,
			AsteroidMinSize = 35,
			AsteroidMaxSize = 200
		};
	}	
	public static EnvironmentParameters DefaultBoss()
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
}
