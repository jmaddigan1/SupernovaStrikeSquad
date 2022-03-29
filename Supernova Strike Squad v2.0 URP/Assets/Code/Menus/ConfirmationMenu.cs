using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmationMenu : MonoBehaviour
{
	Action<bool> confirmationcallback;

	public TextMeshProUGUI textBox;

	void Awake() => gameObject.SetActive(false);

	public void Open(string info, Action<bool> callback)
	{
		confirmationcallback = callback;
		gameObject.SetActive(true);
		textBox.text = info;
	}

	public void Confirm()
	{
		confirmationcallback?.Invoke(true);
		Destroy(gameObject);
	}

	public void Cancel()
	{
		confirmationcallback?.Invoke(false);
		Destroy(gameObject);
	}
}
