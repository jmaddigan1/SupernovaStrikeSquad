using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipProfileScreen : MonoBehaviour
{
	[SerializeField] private ShipSelectScreen shipSelectScreen = null;

	[SerializeField] private TextMeshProUGUI nameText = null;

	public List<WeaponPanel> weapons = new List<WeaponPanel>();

	bool pickShipOpen = false;

	private void Update()
	{
		if (!pickShipOpen) {
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (TryGetComponent<ExtendedMenu>(out ExtendedMenu menu))
				{
					menu.Close();
				}
			}
		}
	}

	public void SelectNewShip()
	{
		pickShipOpen = true;
		Instantiate(shipSelectScreen, GetComponentInParent<Canvas>().transform).Open(OnSelectNewShip);
	}

	public void OnSelectNewShip(ShipType shipName)
	{
		pickShipOpen = false;
		nameText.text = shipName.ToString();

		Player.LocalPlayer.Self.Cmd_UpdateShip(shipName);
	}
}
