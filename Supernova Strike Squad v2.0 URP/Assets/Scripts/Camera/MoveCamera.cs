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
	}

}
