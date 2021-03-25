using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectScreen : MonoBehaviour
{
    Action<string> confirmationCallback;

    public void Open(Action<string> callback)
    {
        confirmationCallback = callback;
    }

    public void Confirm(string weapon)
    {
        Destroy(gameObject);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
