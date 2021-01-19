using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //
    private Transform target;

    //
    private Vector3 offset;

    //
    public void SetTarget(Transform newTarget, Vector3 cameraOffset)
	{
        offset = cameraOffset;
        target = newTarget;

        transform.parent = newTarget;

        transform.localPosition = offset;
    }
}
