using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum ShipType
{
    ShipA,
    ShipB,
    ShipC,
    ShipD,
}

public class ShipSelectScreen : MonoBehaviour
{
    Action<string> confirmationCallback;

    public List<ShipButton> ShipButtons = new List<ShipButton>();

	void Awake()
	{
        Array types = Enum.GetValues(typeof(ShipType));

        for (int index = 0; index < ShipButtons.Count; index++)
        {
            if (index < types.Length)
            {
                ShipButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = types.GetValue(index).ToString();
                ShipButtons[index].ShipType = (ShipType)(types.GetValue(index));
            }
            else
            {
                //  ShipButtons[index].gameObject.SetActive(false);
            }
        }
    }

	public void Open(Action<string> callback)
    {
        confirmationCallback = callback;
    }

    public void Confirm(string weapon)
    {
        confirmationCallback.Invoke(weapon);
        Destroy(gameObject);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
