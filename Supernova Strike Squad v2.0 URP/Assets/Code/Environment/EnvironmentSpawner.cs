using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


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

	[SerializeField] GameObject runnerBounds = null;
	[SerializeField] GameObject areanaBounds = null;

	[SerializeField]
	private GameObject AsteroidPrefab = null;

	[SerializeField]
	private AnimationCurve spawnCurve;

	[SerializeField]
	private AnimationCurve sizeCurve;

	public EnvironmentParameters CurrentEnvironment;

	public List<GameObject> spawnedObjects = new List<GameObject>();

	private GameObject eventBounds;

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

		Rpc_SpawnBounds(environment);

		for (int count = 0; count < environment.AsteroidCount; count++)
		{
			// POSITION
			Vector3 point = GetRandomPosition(environment.EnvironmentSize, environment.EnvironmentType);

			// SCALE
			float size = GetRandomSize(environment.AsteroidMinSize, environment.AsteroidMaxSize);

			// SPAWN
			GameObject go = Instantiate(AsteroidPrefab, point, Quaternion.identity, transform);
			go.transform.localScale = Vector3.one * size;
			spawnedObjects.Add(go);
				
			NetworkServer.Spawn(go);
		}
	}

	[Server]
	public void Clear()
	{
		foreach (GameObject spawnedObject in spawnedObjects) {
			NetworkServer.Destroy(spawnedObject);
		}

		spawnedObjects.Clear();
	}

	[ClientRpc]
	public void Rpc_SpawnBounds(EnvironmentParameters environment)
	{
		if (eventBounds) Destroy(eventBounds.gameObject);
		if (environment.EnvironmentType == EnvironmentType.Square)
		{
			GameObject bounds = Instantiate(runnerBounds);
			bounds.transform.localScale = environment.EnvironmentSize * 2f;
			eventBounds = bounds;
		}
		if (environment.EnvironmentType == EnvironmentType.Sphere)
		{
			GameObject bounds = Instantiate(areanaBounds);
			bounds.transform.localScale = Vector3.one * environment.EnvironmentSize.x * 2;
			eventBounds = bounds;
		}

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
		return Mathf.Clamp(sizeCurve.Evaluate(Random.Range(0.0f, 1.0f)) * max, min, max);
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
}
