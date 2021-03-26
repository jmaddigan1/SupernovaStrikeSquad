using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipProfileScreen : MonoBehaviour
{
	[SerializeField] private ShipSelectScreen shipSelectScreen = null;

	[SerializeField] private TextMeshProUGUI nameText = null;

	public void SelectNewShip()
	{
		Instantiate(shipSelectScreen, GetComponentInParent<Canvas>().transform).Open(OnSelectNewShip);
	}

	public void OnSelectNewShip(string shipName)
	{
		nameText.text = shipName;
	}
}
