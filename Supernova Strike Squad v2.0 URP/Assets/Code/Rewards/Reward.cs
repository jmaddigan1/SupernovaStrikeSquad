using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[System.Serializable]
public class RewardData
{
	public RewardType RewardType;

	public string RewardName;
	public string RewardDescription;
}

public class Reward : NetworkBehaviour
{
	public RewardData RewarData;

	void OnTriggerEnter(Collider other)
	{
		if (isServer)
		{
			RewardManager rewardManager = other.transform.GetComponentInParent<RewardManager>();
			if (rewardManager)
			{
				rewardManager.AddReward(RewarData);
				Destroy(gameObject);
			}
		}
	}
}
