using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
	private WeaponSelectScreen weaponSelectScreen = null;

	public WeaponTypes WeaponTypes;

	void Awake()
	{
		weaponSelectScreen = GetComponentInParent<WeaponSelectScreen>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		weaponSelectScreen.Confirm(WeaponTypes);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		weaponSelectScreen.OnHover(WeaponTypes);
	}
}
