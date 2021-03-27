using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTriggerArea : MonoBehaviour
{
	[SerializeField] private ExtendedMenu menuPrefab = null;
	[SerializeField] private ShipBay shipBay = null;

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

	void OnTriggerEnter(Collider other)
	{
		var enteringPlayer = other.GetComponentInParent<PlayerController>();

		if (Player.LocalPlayer.connectionToClient == enteringPlayer.connectionToClient)
		{
			if (shipBay)
			{
				if (shipBay.ownerID == Player.LocalPlayer.ID) {
					canInteract = true;
				}
			}
			else
			{
				canInteract = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		var enteringPlayer = other.GetComponentInParent<PlayerController>();

		if (Player.LocalPlayer.connectionToClient == enteringPlayer.connectionToClient)
		{
			if (shipBay)
			{
				if (shipBay.ownerID == Player.LocalPlayer.ID) {
					canInteract = false;
				}
			}
			else
			{
				canInteract = false;
			}
		}
	}
}
