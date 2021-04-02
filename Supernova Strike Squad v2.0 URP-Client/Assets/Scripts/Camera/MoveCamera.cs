using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform CameraTarget;


	private void LateUpdate()
	{
		if (CameraTarget)
		{
			transform.position = CameraTarget.position;
		}

		//if (CameraTarget)
		//{
		//	Vector3 targetPosition = CameraTarget.TransformPoint(new Vector3(0, 0, 0));
		//	transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.1f);
		//}
	}

}
