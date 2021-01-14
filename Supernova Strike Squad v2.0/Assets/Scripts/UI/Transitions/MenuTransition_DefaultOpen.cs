using System.Collections;
using UnityEngine;

public class MenuTransition_DefaultOpen : MenuTransition
{
	public override IEnumerator Play()
	{
		float screen = Screen.height;
		bool waiting = true;

		Tween.Instance.EaseOut_Transform_ElasticY(transform, -screen,0, 1, 0, () => {
			waiting = false;
		});

		while (waiting) yield return null;
	}
}
