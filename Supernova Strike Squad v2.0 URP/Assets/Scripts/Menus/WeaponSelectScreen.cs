using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum WeaponTypes
{
    // Shoots heat seeking projectiles
    RocketsLauncher,

    // Long beam of dot damage
    LaserBeam,

    // Fast shooting MiniGun good against shield enemies
    EnergyMiniGun,

    // Fast shooting MiniGun good against non shielded enemies
    ExplosiveMiniGun,

    // Low range but high damage
    Shotgun,

    // Long charge time but powerful single target projectile
    Railgun,

    // Arcs to nearby target
    Arc,

    // Charge and shoot a growing projectile
    Charger,

    // Summons 2 small drones and creates a melee weapon for the player ship
    TurretsAndRam,

    // Cannot shoot but created a powerful shield for the player
    ShieldDrones,  
    
    // Cannot shoot but greatly speed up the player
    SpeedDrones,

    // Shoot a powerful beam that works well against all targets
    TriBeam,

    // Shoot a melee pulse of energy around the player
    EnergyPulse,
}

public class WeaponSelectScreen : MonoBehaviour
{
    public List<WeaponButton> weaponButtons = new List<WeaponButton>();

    public WeaponPanel WeaponPanel = null;

    public WeaponTypes CurrentWeapon;

    Action<WeaponTypes> confirmationcallback;

    private void Awake()
	{
        Array types = Enum.GetValues(typeof(WeaponTypes));

		for (int index = 0; index < weaponButtons.Count; index++)
		{
			if (index < types.Length )
            {
                weaponButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = types.GetValue(index).ToString();
                weaponButtons[index].WeaponTypes = (WeaponTypes)(types.GetValue(index));
            }
            else
            {
               //  weaponButtons[index].gameObject.SetActive(false);
            }
        }
    }

    public void Open(Action<WeaponTypes> callback, WeaponTypes currentWeapon)
    {
        CurrentWeapon = currentWeapon;
        confirmationcallback = callback;

        // TODO: Update
    }

    public void Confirm(WeaponTypes weapon)
    {
        confirmationcallback.Invoke(weapon);
        Destroy(gameObject);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }

    public void OnHover(WeaponTypes weapon)
	{
        WeaponPanel.UpdateWeaponInfo(weapon);
    }
}
