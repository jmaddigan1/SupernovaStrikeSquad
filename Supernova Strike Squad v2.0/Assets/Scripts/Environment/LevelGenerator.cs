using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// The Level Generator build a given environment when given EnvironmentData
public class LevelGenerator : NetworkBehaviour
{
	// Global Members
	// The Level LevelGenerator Singleton
	public static LevelGenerator Instance;

	// Editor References
	[SerializeField]
	private GameObject asteroidPrefab = null;

	// Private Members
	private EnvironmentData environment;

	// Public Members
	public AnimationCurve AsteroidSizeCurve;
	public AnimationCurve ObjectSpawningDistanceCurve;

	private List<GameObject> environmentObjects = new List<GameObject>();

	void Start()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}

		Instance = this;
	}

	[Server]
	void BuildEnvironment(EnvironmentData data)
	{
		environment = data;

		for (int count = 0; count < data.AsteroidCount; count++)
		{
			GameObject go = Instantiate(asteroidPrefab);

			float x = Random.Range(-1.0f, 1.0f);
			float y = Random.Range(-1.0f, 1.0f);
			float z = Random.Range(-1.0f, 1.0f);

			Vector3 direction = new Vector3(x, y, z).normalized;

			float min = ObjectSpawningDistanceCurve.Evaluate(Random.Range(0f, 1f)) * data.Size;

			Vector3 pos = direction * Random.Range(min, data.Size);

			go.transform.position = pos;

			// SCALE
			float scale = Mathf.Clamp(AsteroidSizeCurve.Evaluate(Random.Range(0.0f, 1.0f)) * data.MaxAsteroidSize, data.MinAsteroidSize, data.MaxAsteroidSize);

			go.transform.localScale = Vector3.one * scale;

			// SPAWN
			NetworkServer.Spawn(go);

			// KEEP TRACK
			environmentObjects.Add(go);
		}
	}

	[Server]
	void RemoveEnvironment()
	{
		foreach (GameObject gameObject in environmentObjects)
		{
			NetworkServer.Destroy(gameObject);
		}
	}

	void OnDrawGizmos()
	{
		if (environment != null) Gizmos.DrawWireSphere(transform.position, environment.Size);
	}

	#region Level Generator Wrapper Methods

	public static void Build(EnvironmentData data)
	{ if (ValidSingleton()) Instance.BuildEnvironment(data); }

	public static void Remove()
	{ if (ValidSingleton()) Instance.RemoveEnvironment(); }

	static bool ValidSingleton()
	{
		if (Instance == null)
		{
			Debug.LogError("ERROR: There is no 'LevelGenerator' in the scene");
			return false;
		}

		return true;
	}

	#endregion
}
