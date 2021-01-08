using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Menu startingMenu;

	// Start is called before the first frame update
	void Start()
	{
		if (startingMenu) startingMenu.Open();
	}
}
