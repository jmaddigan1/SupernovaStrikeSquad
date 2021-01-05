using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColorObjectWithAuthority : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		if (hasAuthority)
		{
			foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
			{
				renderer.material.color = Color.blue;
			}
		}
    }
}
