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

public class ShipSelectScreen : Menu
{
    Action<ShipType> confirmationCallback;

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
        }
    }

	void Update()
	{
        if (PlayerController.Interacting)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cancel();
            }
        }
    }

    public void Open(Action<ShipType> callback)
    {
        confirmationCallback = callback;

        // Hide this menu till the animation starts
        GetComponent<CanvasGroup>().alpha = 0f;
        OpenMenu(null, false);
    }
    
    // NOTE: Confirm is called by clicking on a 'ShipButton'
    public void Confirm(ShipType ship)
    {
        // Setup the Destroy to happen after the menu close animation
        callback = () => {
            confirmationCallback.Invoke(ship);
            Destroy(gameObject);
        };

        CloseMenu(); 
    }

    public void Cancel()
    {
        // Setup the Destroy to happen after the menu close animation
        callback = () => { Destroy(gameObject); };
        CloseMenu();  
    }   
}
