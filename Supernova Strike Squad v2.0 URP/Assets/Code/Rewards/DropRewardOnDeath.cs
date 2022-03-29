using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Health))]
public class DropRewardOnDeath : NetworkBehaviour
{
    [SerializeField]
    private GameObject RewardToDrop = null;

	public override void OnStartServer()
	{
		base.OnStartServer();

		Health health = GetComponent<Health>();
		health.OnHealthUpdate += OnDeath;
	}

	[Server]
	public void OnDeath(float value, float maxValue)
	{
		if (value <= 0)
		{
			NetworkServer.Spawn(Instantiate(RewardToDrop, transform.position, transform.rotation));
		}
	}
}
