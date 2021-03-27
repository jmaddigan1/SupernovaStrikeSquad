using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPanel : MonoBehaviour
{
	[SerializeField] private WeaponSelectScreen selectScreen = null;

	[SerializeField] private TextMeshProUGUI descriptionText = null;
	[SerializeField] private TextMeshProUGUI titleText = null;

	public WeaponTypes myType;

	public int playerWeaponSlotIndex;

	void Awake()
	{
		myType = Player.LocalPlayer.Self.Weapons[playerWeaponSlotIndex];
		UpdateWeaponInfo(myType);
	}

	public void SelectNewWeapon()
	{
		Instantiate(selectScreen, GetComponentInParent<Canvas>().transform).Open(OnSelectNewWeapon, WeaponTypes.Arc);
	}

	public void OnSelectNewWeapon(WeaponTypes weapon)
	{
		UpdateWeaponInfo(weapon);
		Player.LocalPlayer.Self.Cmd_UpdateWeapon(weapon, playerWeaponSlotIndex);
	}

	public void UpdateWeaponInfo(WeaponTypes weaponName)
	{
		myType = weaponName;
		titleText.text = weaponName.ToString();
	}
}
