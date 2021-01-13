using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    public void SetTarget(Transform parent)
	{
        transform.parent = parent;
        transform.localPosition = new Vector3(0, 4, -5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
