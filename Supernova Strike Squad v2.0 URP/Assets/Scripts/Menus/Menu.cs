using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public static Menu currentMenu;

	public void Open(Menu targetMenu)
	{
		Debug.Log(targetMenu.name);

		currentMenu = this;

		targetMenu.gameObject.SetActive(true);
		transform.gameObject.SetActive(false);
	}
}
