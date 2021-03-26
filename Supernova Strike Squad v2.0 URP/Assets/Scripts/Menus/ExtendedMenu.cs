using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedMenu : Menu
{
	Action callback = null;

	public void Open(Action callback = null)
	{
		this.callback = callback;
	}

	public void Close()
	{
		callback?.Invoke();
		Destroy(gameObject);
	}
}
