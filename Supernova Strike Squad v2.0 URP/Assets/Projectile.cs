using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
	Rigidbody rb;
	private void Start()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();

		if (isServer)
		{
			rb = rigidbody;
			rb.velocity = transform.forward * 500;

			Destroy(gameObject, 1f);
		}
		else
		{
			Destroy(rigidbody);
		}
	}
}
