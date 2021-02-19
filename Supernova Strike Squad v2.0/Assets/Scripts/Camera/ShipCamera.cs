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
		Target = target;
		transform.SetParent(target);
		transform.localPosition = offset;
	}

	void Update()
	{
		//// Blocker
		//if (Target = null) return;

		//Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));

		//float camPosX = (input.x * 1.5f);
		//float camPosY = (input.y * 0.5f) + 1.0f;
		//float camPosZ = -5.5f;

		//transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(camPosX, camPosY, camPosZ), Time.deltaTime * 4);
	}
}
