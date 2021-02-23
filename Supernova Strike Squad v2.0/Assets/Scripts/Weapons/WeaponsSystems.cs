using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponsSystems : NetworkBehaviour
{
	[SerializeField]
	// weaponPrefabs is a list of weapons the player can use
	// these are used to build the WeaponsDictionary that the player can pull from
	// NOTE: These should never be directly assigned to the player
	private List<WeaponBase> weaponPrefabs = new List<WeaponBase>();

	// This is the parent of any player weapons
	public Transform WeaponAnchor = null;

	// The WeaponDictionary is a list of weapons we are reference with a WeaponType
	public Dictionary<WeaponType, WeaponBase> WeaponDictionary = new Dictionary<WeaponType, WeaponBase>();

	public WeaponBase CurrentWeapon;

	void Awake()
	{
		foreach (WeaponBase weapon in weaponPrefabs)
		{
			if (WeaponDictionary.ContainsKey(weapon.Type))
			{
				Debug.LogError($"ERROR: This WEAPON has already been registered! ({weapon.Type})");
			}
			else
			{
				foreach (WeaponBase w in WeaponDictionary.Values)
				{
					if (weapon.EquipKey == w.EquipKey)
					{
						Debug.LogError($"ERROR: This KEY has already been registered! ({weapon.EquipKey})");
						continue;
					}
				}

				WeaponDictionary.Add(weapon.Type, weapon);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (hasAuthority == false) return;

		Debug.Log("adsd");

		foreach (WeaponBase weapon in WeaponDictionary.Values)
		{
			if (Input.GetKeyDown(weapon.EquipKey))
			{
				// And it is not the same weapon we are using
				if (weapon != CurrentWeapon)
				{
					EquipNewWeapon(weapon.Type);
				}
			}
		}

		if (CurrentWeapon != null)
		{
			if (Input.GetKeyUp(CurrentWeapon.ShootKey))
				CurrentWeapon.OnShootUp();

			if (Input.GetKeyDown(CurrentWeapon.ShootKey))
				CurrentWeapon.OnShootDown();
		}
	}

	public void EquipNewWeapon(WeaponType weapon)
	{
		if (WeaponDictionary.ContainsKey(weapon) == false) {
			Debug.LogError($"ERROR: That weapon is not registered! ({weapon})");
		}

		CmdSpawnWeapon(weapon);
	}

	[Command]
	public void CmdSpawnWeapon(WeaponType weapon)
	{
		NetworkServer.Spawn(Instantiate(WeaponDictionary[weapon].gameObject, WeaponAnchor), connectionToClient);
	}

	[Command]
	public void CmdRemoveWeapon(GameObject weapon) => NetworkServer.Destroy(weapon);
}