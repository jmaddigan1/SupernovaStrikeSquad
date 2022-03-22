using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
	public KeyCode InteractKey = KeyCode.Mouse0;

	// What Menu this trigger will spawn
	public Menu MenuToSpawn = null;

	// The range of this trigger
	public float Range = 5.0f;
	public int OwnerID = -1;

	bool canInteract = false;

	Menu createdMenu = null;

	void Awake()
	{
		var collider = GetComponent<SphereCollider>();
		collider.radius = Range;
	}

	void Update()
	{
		if (canInteract)
		{
			// If we press the interact key
			// The local player is not interacting with anything
			if (Input.GetKeyDown(InteractKey) && !PlayerController.Interacting)
			{
				PlayerController.Interacting = true;

				SpawnMenu();
			}
		}
	}

	void SpawnMenu()
	{
		createdMenu = Instantiate(MenuToSpawn, GetCanvasAnchor());
		createdMenu.OpenMenu(null, false, () => { 
			OnCloseMenu(); 
		});
	}

	void OnCloseMenu()
	{
		PlayerController.Interacting = false;
		Destroy(createdMenu.gameObject);
	}

	Transform GetCanvasAnchor()
	{
		return GameObject.FindGameObjectWithTag("MainCanvas").transform;
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.GetComponentInParent<PlayerController>();

		if (IsLocalPlayer(player))
		{
			// If we NEED to match an ID with a specific player
			if (MatchOwnerID())
			{
				// If the local players ID is this areas ID
				canInteract = OwnerID == Player.LocalPlayer.ID;
			}
			else
			{
				canInteract = true;
			}
		}
	}
	void OnTriggerExit(Collider other)
	{
		PlayerController player = other.GetComponentInParent<PlayerController>();

		if (IsLocalPlayer(player))
		{
			canInteract = false;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, Range);
	}

	public bool IsLocalPlayer(PlayerController player)
	{
		return Player.LocalPlayer.connectionToClient == player.connectionToClient;
	}

	public bool MatchOwnerID()
	{
		return OwnerID != -1;
	}
}
