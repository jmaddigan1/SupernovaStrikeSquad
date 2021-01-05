using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelGenerator : NetworkBehaviour
{
	[SerializeField] private GameObject asteroidPrefab;

	//
	public AnimationCurve AsteroidSizeCurve;

	public override void OnStartServer()
	{
		// Only do this on the server
		if (isServer == false) return;

		EnvironmentData environmentData = new EnvironmentData
		{
			Size = 100,
			AsteroidCount = 50,
			MinAsteroidSize = 5,
			MaxAsteroidSize = 55,
		};

		BuildEnvironment(environmentData);
	}

	void BuildEnvironment(EnvironmentData data)
	{
		for (int count = 0; count < data.AsteroidCount; count++)
		{
			GameObject go = Instantiate(asteroidPrefab);

			float scale = AsteroidSizeCurve.Evaluate(Random.Range(0.0f, 1.0f)) * data.MaxAsteroidSize;

			go.transform.position = new Vector3(
				 UnityEngine.Random.Range(-data.Size, data.Size),
				 UnityEngine.Random.Range(-data.Size, data.Size),
				 UnityEngine.Random.Range(-data.Size, data.Size)
			);

			//scale = Mathf.Clamp(scale, data.MinAsteroidSize, scale * data.MaxAsteroidSize);

			go.transform.localScale = Vector3.one * scale;
			NetworkServer.Spawn(go);
		}
	}
}
