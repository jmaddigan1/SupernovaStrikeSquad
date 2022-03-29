using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions_CloseDefault : MenuTransition
{
	public override IEnumerator Play(Menu owner)
	{
		yield return new WaitForSeconds(0.0f);
		owner.gameObject.SetActive(false);
	}
}
