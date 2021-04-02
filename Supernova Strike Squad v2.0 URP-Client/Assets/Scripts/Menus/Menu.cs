using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public static Menu currentMenu;

	public virtual void Open(Menu targetMenu = null)
	{
		currentMenu = this;

		targetMenu.gameObject.SetActive(true);
		transform.gameObject.SetActive(false);
	}
}
