using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTriggerArea : MonoBehaviour
{
	[SerializeField] private ExtendedMenu menuPrefab = null;

	bool canInteract = false;

	ExtendedMenu menu;

	void Update()
	{
		if (canInteract)
		{
			// If we press the interact key
			if (Input.GetKeyDown(KeyCode.Mouse0) && PlayerController.Interacting == false) 
			{
				menu = Instantiate(menuPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);
				menu.Open(OnMenuClose);

				PlayerController.Interacting = true;

				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}

	void OnMenuClose()
	{
		PlayerController.Interacting = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void OnTriggerEnter(Collider other) => canInteract = true;
	void OnTriggerExit(Collider other) => canInteract = false;
}
