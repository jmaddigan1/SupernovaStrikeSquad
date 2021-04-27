using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
	public List<RewardData> Rewards = new List<RewardData>();

	public void AddReward(RewardData reward)
	{
		Rewards.Add(reward);
	}
}
