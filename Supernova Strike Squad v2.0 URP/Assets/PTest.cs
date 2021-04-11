using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTest : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * 150;
		Destroy(gameObject, 1.0f);
	}
}
