using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions_OpenDefault : MenuTransition
{
	public override IEnumerator Play(Menu owner)
	{
		yield return new WaitForSeconds(0.0f);
		owner.gameObject.SetActive(true);
	}
}
