using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
	public static CharacterCamera Instance;

	public Transform Target;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}

		Instance = this;
	}

	void Update()
	{
		// Blocker
		if (Target = null) return;

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));

		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(input.x / 2 , 3, -3 + (input.y * 0.2f)), Time.deltaTime * 2);

		// transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, input.y, 0), Time.deltaTime * 10);

	}

	public void SetTarget(Transform target, Vector3 offset)
	{
		transform.SetParent(target);
		transform.localPosition = offset;
	}
}