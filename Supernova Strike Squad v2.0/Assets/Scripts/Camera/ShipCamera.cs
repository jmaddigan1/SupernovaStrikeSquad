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

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));

        float camPosX = (input.x * 1.0f);
        float camPosY = 1;
        float camPosZ = -7.5f;

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(camPosX, camPosY, camPosZ), Time.deltaTime * 2);

    }
}
