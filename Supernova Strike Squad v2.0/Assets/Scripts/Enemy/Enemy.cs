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
			 UnityEngine.Random.Range(-150, 150),
			 UnityEngine.Random.Range(-150, 150),
			 UnityEngine.Random.Range(-150, 150)
			);

		Compass.Instance.AddTarget(transform);
	}

	void OnDestroy()
	{
		OnDeath?.Invoke(this);
	}
}
