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

			float scale = AsteroidSizeCurve.Evaluate(Random.Range(0.0f, 1.0f)) * data.MaxAsteroidSize;

			go.transform.position = new Vector3(
				 UnityEngine.Random.Range(-data.Size, data.Size),
				 UnityEngine.Random.Range(-data.Size, data.Size),
				 UnityEngine.Random.Range(-data.Size, data.Size)
			);

			go.transform.localScale = Vector3.one * scale;
			NetworkServer.Spawn(go);

			environmentObjects.Add(go);
		}
	}

	[Server]
	void RemoveEnvironment( )
	{
		foreach (GameObject gameObject in environmentObjects) {
			NetworkServer.Destroy(gameObject);
		}
	}

	void OnDrawGizmos()
	{
		if (environment !=null) Gizmos.DrawWireSphere(transform.position, environment.Size * 2);
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
