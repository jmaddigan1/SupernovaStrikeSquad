using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipCamera : MonoBehaviour
{
    public static ShipCamera Instance;

    public Transform Target;
    public Transform Cam;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
    }

    public void SetTarget(Transform target, Vector3 offset)
    {
        transform.SetParent(target);
        transform.localPosition = offset;
    }

    void Update()
    {
        // Blocker
        if (Target = null) return;

        Vector2 input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxisRaw("Horizontal"));
        Vector3 target = new Vector3(input.x, input.y, 0);

        Vector3 pos = Vector3.Lerp(Cam.transform.localScale, target, Time.deltaTime * 5);

        Cam.transform.localPosition = pos;

        if (true)
		{

		}

    }
}
