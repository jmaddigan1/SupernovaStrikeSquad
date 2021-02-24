using System;
using UnityEngine;
using Mirror;

public class Enemy : NetworkBehaviour
{
	public Action<Enemy> OnDeath;

	public EnemyID ID;

	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(
			 UnityEngine.Random.Range(-100f, 100f),
			 UnityEngine.Random.Range(-100f, 100f),
			 UnityEngine.Random.Range(-100f, 100f)
			);

		Compass.Instance?.AddTarget(transform, Color.red);
	}

	void OnDestroy()
	{
		OnDeath?.Invoke(this);
	}
}
