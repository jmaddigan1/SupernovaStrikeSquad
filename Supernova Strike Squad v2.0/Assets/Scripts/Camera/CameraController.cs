using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform cameraTransform = null;

	//
	private KeyCode freeCameraKey = KeyCode.LeftShift;

	private Transform target;

	private Vector3 offset;

	private bool freeCamera;

	//
	public void SetTarget(Transform newTarget, Vector3 cameraOffset)
	{
		offset = cameraOffset;
		target = newTarget;

		transform.parent = newTarget;

		transform.localPosition = Vector3.zero;

		cameraTransform.localPosition = offset;
	}

	void FixedUpdate()
	{
		freeCamera = Input.GetKey(freeCameraKey);

		if (freeCamera)
		{
			float yaw = 3 * -Input.GetAxis("Mouse X");

			transform.Rotate (0, yaw, 0);
		}
		else
		{
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.fixedDeltaTime * 5);
		}
	}
}
