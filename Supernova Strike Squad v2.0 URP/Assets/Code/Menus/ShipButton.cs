using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipButton : MonoBehaviour, IPointerClickHandler
{
	private ShipSelectScreen shipSelectScreen = null;

	public ShipType ShipType;

	void Awake()
	{
		shipSelectScreen = FindObjectOfType<ShipSelectScreen>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		shipSelectScreen.Confirm(ShipType);
	}
}
