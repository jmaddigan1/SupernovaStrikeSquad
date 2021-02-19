using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reticle
public class Targeter : MonoBehaviour
{
	// The Targeter Singleton
	public static Targeter Instance;

	// This is the up element the represents the players reticle
	public Transform Reticle = null;

	// Awake is called when this script is created
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}

		Instance = this;
	}

	// Update is called once per frame
	void Update()
	{
		//Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		//float height = Screen.height / 4;

		//Vector3 CurPos = Reticle.transform.localPosition;
		//Vector3 TarPos = input * height;

		//Reticle.transform.localPosition = Vector3.Lerp(CurPos, TarPos, Time.deltaTime * 3);
	}
}
