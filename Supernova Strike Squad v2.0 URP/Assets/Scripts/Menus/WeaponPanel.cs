using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPanel : MonoBehaviour
{
	[SerializeField] private WeaponSelectScreen selectScreen = null;

	[SerializeField] private TextMeshProUGUI descriptionText = null;
	[SerializeField] private TextMeshProUGUI titleText = null;

	public void SelectNewWeapon()
	{
		Instantiate(selectScreen, GetComponentInParent<Canvas>().transform).Open(OnSelectNewWeapon, WeaponTypes.Arc);
	}

	public void OnSelectNewWeapon(WeaponTypes weaponName)
	{
		UpdateWeaponInfo(weaponName);
	}

	public void UpdateWeaponInfo(WeaponTypes weaponName)
	{
		titleText.text = weaponName.ToString();
	}
}
