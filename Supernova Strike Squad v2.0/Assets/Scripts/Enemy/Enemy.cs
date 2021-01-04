using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Action<Enemy> OnDeath;

	public EnemyID ID;

	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(
			 UnityEngine.Random.Range(-15, 15),
			 UnityEngine.Random.Range(-15, 15),
			 UnityEngine.Random.Range(-15, 15)
			);
	}

	[ContextMenu("KILL")]
	public void Die()
	{
		OnDeath.Invoke(this);

		Destroy(gameObject);
	}
}
