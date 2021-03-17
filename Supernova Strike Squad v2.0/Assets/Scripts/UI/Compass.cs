using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class CompassTarget : MonoBehaviour
//{

//}

public class Compass : MonoBehaviour
{
	public static Compass Instance;

	[SerializeField]
	private Camera compassCamera = null;

	[SerializeField]
	private Transform targetPrefab = null;

	private List<Transform> targets = new List<Transform>();
	private List<Transform> targetImages = new List<Transform>();

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}

		Instance = this;
	}

	// Update is called once per frame
	void Update()
	{
		// TODO: check is the target is behind us

		if (PlayerConnection.LocalPlayer.Object.PlayerObject == null) return;

		for (int index = 0; index < targets.Count; index++)
		{
			if (targets[index] == null)
			{
				targets.RemoveAt(index);
				Destroy(targetImages[index].gameObject);
				targetImages.RemoveAt(index);
				index--;
				continue;
			}
			else
			{
				// Set the position of the targets graphic
				targetImages[index].position = compassCamera.WorldToScreenPoint(targets[index].position);

				if (GetAngleFromPlayer(targets[index]) > 90)
				{
					targetImages[index].localPosition = Vector3.up * Screen.height * 2;
				}
				else
				{
					Vector2 t = new Vector2(targetImages[index].localPosition.x, targetImages[index].localPosition.y);

					if (t.magnitude > 400)
					{
						Vector2 direction = t.normalized;
						targetImages[index].localPosition = direction * 400;
					}
				}
			}
		}
	}

	public float GetAngleFromPlayer(Transform target)
	{
		Transform playerObj = PlayerConnection.LocalPlayer.Object.PlayerObject.transform;
		Vector3 direction = target.position - playerObj.position;
		float angle = Vector3.Angle(direction, playerObj.forward);
		return angle;
	}

	public void AddTarget(Transform newTarget, Color color)
	{
		targetImages.Add(Instantiate(targetPrefab, transform));
		targetImages[targetImages.Count - 1].GetComponent<Image>().color = color;

		targets.Add(newTarget);
	}
}
