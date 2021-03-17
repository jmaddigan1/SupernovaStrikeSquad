using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// The Weapons Systems Manages the local players Weapon
public class WeaponsSystems : NetworkBehaviour
{
	// weaponPrefabs is a list of weapons the player can use..
	// these are used to build the WeaponsDictionary that the player can pull from

	// NOTE: These should never be directly assigned to the player
	[SerializeField] List<WeaponBase> weaponPrefabs = new List<WeaponBase>();


	// The WeaponDictionary is a list of weapons we are reference with a WeaponType
	public Dictionary<WeaponType, WeaponBase> WeaponDictionary = new Dictionary<WeaponType, WeaponBase>();


	// This is the parent of any player weapons
	public Transform WeaponAnchor = null;


	// The current weapon the local player is using
	public WeaponBase CurrentWeapon;


	void Awake() => BuildWeaponDictionary();

	// Update checks if the player wants to equip a new weapon
	// Update manages if the player is shooting
	void Update()
	{
		if (hasAuthority == false) return;

		// If the local client has a weapon
		if (CurrentWeapon)
		{

			// Start shooting
			if (Input.GetKeyUp(CurrentWeapon.ShootKey)) CurrentWeapon.OnShootUp();


			// Stop Shooting
			if (Input.GetKeyDown(CurrentWeapon.ShootKey)) CurrentWeapon.OnShootDown();

		}

		// Loop through each weapon and check if its equip key is pressed
		foreach (WeaponBase weapon in WeaponDictionary.Values)
		{
			if (Input.GetKeyDown(weapon.EquipKey))
			{

				// And it is not the same weapon we are using
				CmdEquipNewWeapon(weapon.Type);

			}
		}
	}

	// Build the Weapon Dictionary
	public void BuildWeaponDictionary()
	{
		// Loop trough the weapon prefabs
		foreach (WeaponBase weapon in weaponPrefabs)
		{

			// If we have already registered a weapon we want to print out an error
			if (WeaponDictionary.ContainsKey(weapon.Type))
			{
				Debug.LogError($"ERROR: This WEAPON has already been registered! ({weapon.Type})");
			}
			else
			{

				// Loop through cheat weapon and make sure each weapon key is unique
				foreach (WeaponBase w in WeaponDictionary.Values)
				{
					if (weapon.EquipKey == w.EquipKey)
					{
						Debug.LogError($"ERROR: This KEY has already been registered! ({weapon.EquipKey})");
						continue;
					}
				}

				// This is a valid weapon so add it to our WeaponDictionary
				WeaponDictionary.Add(weapon.Type, weapon);

			}

		}

	}

	[Command]
	public void CmdEquipNewWeapon(WeaponType weapon)
	{
		if (WeaponDictionary.ContainsKey(weapon) == false)
		{
			Debug.LogError($"ERROR: That weapon is not registered! ({weapon})");
		}

		// TODO: Make sure its not the weapon were using

		CmdSpawnWeapon(weapon);
	}

	[Server]
	public void CmdSpawnWeapon(WeaponType weapon)
	{
		// Remove our last weapon
		if (CurrentWeapon)
			NetworkServer.Destroy(CurrentWeapon.gameObject);

		// Spawn in the new weapon
		GameObject go = Instantiate(WeaponDictionary[weapon].gameObject, WeaponAnchor);
		CurrentWeapon = go.GetComponent<WeaponBase>();

		// Spawn the weapon to all clients
		NetworkServer.Spawn(go, connectionToClient);
	}
}