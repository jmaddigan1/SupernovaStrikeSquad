using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTriggerArea : MonoBehaviour
{
	[SerializeField] private Menu menuToOpen = null;

	bool canOpen = false;

	private void Update()
	{
		// The player is standing in the trigger area
		if (canOpen)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				menuToOpen.gameObject.SetActive(!menuToOpen.gameObject.activeSelf);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		canOpen = true;
	}

	void OnTriggerExit(Collider other)
	{
		canOpen = false;
		menuToOpen.gameObject.SetActive(false);
	}
}
