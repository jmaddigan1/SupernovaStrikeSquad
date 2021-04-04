using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions_DropDown : MenuTransition
{
	public bool Enter = true;

	public float Direction = 0.75f;

	public override IEnumerator Play(Menu owner)
	{
		bool waiting = true;

		if (Enter)
		{
			Tween.Instance.EaseOut_Transform_ElasticY(owner.transform, Screen.height, 0, Direction, 0, () => {waiting = false;});
		}
		else
		{
			Tween.Instance.EaseIn_Transform_ElasticY(owner.transform, 0, Screen.height, Direction, 0, () => { waiting = false; });
		}

		while (waiting) yield return null;
	}
}
