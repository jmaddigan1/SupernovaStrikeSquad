using System.Collections.Generic;
using UnityEngine;
using Mirror;

// All Weapons inherit from this class
public abstract class WeaponBase : NetworkBehaviour
{
	// The 'EquipKey' is a Debug key we can use to test this weapon
	public KeyCode EquipKey;

	// The 'ShootKey' is the key we use to shoot
	public KeyCode ShootKey;


	// The Type of weapon this is
	// NOTE: This must be unique
	public WeaponType Type;


	// If this weapon shooting
	public bool Shooting;


	// Manage pressing the Shooting button
	public virtual void OnShootUp() => Shooting = false;
	public virtual void OnShootDown() => Shooting = true;


	// Manage Equipping and UnEquipping this weapon
	public virtual void OnEquip() { }
	public virtual void OnUnequip() { }


	// When this weapon is spawned for the local client
	public override void OnStartAuthority()
	{
		// Get the Local clients weapon systems
		var weaponsSystem = PlayerConnection.GetLocalPlayersWeaponsSystem();


		// Set this weapon to the local players weapon anchor
		if (weaponsSystem) transform.SetParent(weaponsSystem.WeaponAnchor);


		// Set this weapon to the lo players weapon
		if (weaponsSystem) weaponsSystem.CurrentWeapon = this;


		// Run the 'OnEquip' logic for this weapon
		OnEquip();
	}

	// When the local player is destroyed
	public override void OnStopAuthority()
	{
		// Run the 'OnUnequip' logic for this weapon
		OnUnequip();
	}


	#region Weapon Base Utilities

	[Server]
	public void IgnoreColliders(Collider shipCollider, Collider projectileCollider)
	{
		Physics.IgnoreCollision(shipCollider, projectileCollider);
	}

	#endregion
}
