using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class Weapon : NetworkBehaviour
{
	public WeaponTypes WeaponTypes;

	public KeyCode ShootKey = KeyCode.Mouse0;

	protected bool shooting = false;

	public override void OnStartAuthority()
	{
		WeaponsSystem.LocalWeaponsSystem.CurrentWeapon = this;
	}

	private void Update()
	{
		if (hasAuthority) WeaponUpdate();
	}

	public virtual void WeaponUpdate() { }

	public virtual void OnEquip() { }
	public virtual void OnUnequip() { }

	public virtual void OnStartShooting() { shooting = true; }
	public virtual void OnStopShooting() { shooting = false; }
}
