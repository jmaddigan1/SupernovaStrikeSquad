using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipProfileScreen : Menu
{
	[SerializeField] private ShipSelectScreen shipSelectScreen = null;

	[SerializeField] private TextMeshProUGUI nameText = null;

	public List<WeaponPanel> weapons = new List<WeaponPanel>();

	bool pickingShip = false;

	private void Start()
	{
		UpdateCursor(CursorLockMode.None, true);
	}

	private void Update()
	{     	
		// If the player is interacting with a menu
		if (PlayerController.Interacting && pickingShip == false)
		{
			if (Input.GetKeyDown(KeyCode.Escape)) {
				CloseMenu();
			}
		}
	}

	public override void CloseMenu()
	{
		UpdateCursor(CursorLockMode.Locked, false);

		base.CloseMenu();
	}

	public void SelectNewShip()
	{
		// We are selecting a new ship
		pickingShip = true;

		// Spawn in the ship picker menu
		Instantiate(shipSelectScreen, GetComponentInParent<Canvas>().transform).Open(OnSelectNewShip);
	}

	public void OnSelectNewShip(ShipType shipName)
	{
		pickingShip = false;
		nameText.text = shipName.ToString();

		Player.LocalPlayer.Self.Cmd_UpdateShip(shipName);
	}
}
