using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerLook : NetworkBehaviour
{
	[SerializeField] private float sensX;
	[SerializeField] private float sensY;

	[SerializeField] private Transform cam = null;
	[SerializeField] private Transform orientation = null;

	float mouseX;
	float mouseY;

	float multiplier = 0.01f;

	float xRotation;
	float yRotation;

	public override void OnStartAuthority()
	{	
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		cam = FindObjectOfType<MoveCamera>().transform;
	}

	void Update()
	{
		if (!PlayerController.Interacting && cam)
		{
			mouseX = Input.GetAxisRaw("Mouse X");
			mouseY = Input.GetAxisRaw("Mouse Y");

			yRotation += mouseX * sensX * multiplier;
			xRotation -= mouseY * sensY * multiplier;

			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
			orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
		}		
	}
}
