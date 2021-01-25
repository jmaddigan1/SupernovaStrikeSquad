using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class WeaponBase : NetworkBehaviour
{
	public KeyCode EquipKey;
	public KeyCode ShootKey;

	public WeaponType Type;

	public List<Transform> Barrels = new List<Transform>();

	public override void OnStartAuthority()
	{
		var objManager = PlayerConnection.LocalPlayer.PlayerObjectManager;
		var weaponsSystem = objManager.PlayerObject.GetComponent<WeaponsSystems>();

		//
		if (weaponsSystem) transform.SetParent(weaponsSystem.WeaponAnchor);

		//
		if (weaponsSystem) weaponsSystem.CurrentWeapon = this;

		OnEquip();
	}

	public override void OnStopAuthority()
	{
		OnUnequip();
	}

	public virtual void OnEquip() { }
	public virtual void OnUnequip() { }

	public virtual void OnShootDown() { }
	public virtual void OnShootUp() { }

	void OnDrawGizmos()
	{
		foreach (Transform barrel in Barrels)
		{
			Gizmos.DrawCube(barrel.position, Vector3.one * 0.5f);
			Gizmos.DrawLine(barrel.position, barrel.position + barrel.forward);
		}
	}
}
