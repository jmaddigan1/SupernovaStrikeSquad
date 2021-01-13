﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelGenerator : NetworkBehaviour
{
	// Global Members
	public static LevelGenerator Instance;


	// Editor References
	[SerializeField]
	private GameObject asteroidPrefab = null;


	// Public Members
	public AnimationCurve AsteroidSizeCurve;

	
	private void Awake()
	{
		if (Instance == null) { Instance = this; }
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	void BuildEnvironment(EnvironmentData data)
	{
		if (isServer == false) return;

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

	#region Level Generator Wrapper Methods

	public static void Build(EnvironmentData data)
	{
		if (Instance == null)
		{
			Debug.LogError("ERROR: There is no 'LevelGenerator' in the scene");
		}
		else
		{
			Instance.BuildEnvironment(data);
		}
	}

	public static void Remove()
	{

	}

	#endregion
}
