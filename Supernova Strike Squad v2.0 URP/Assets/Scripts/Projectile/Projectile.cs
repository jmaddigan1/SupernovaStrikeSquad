using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
	Rigidbody rb;

	public override void OnStartServer()
	{
		Destroy(gameObject, 1f);
	}

	public override void OnStartClient()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rb = rigidbody;

		rb.velocity = transform.forward * 500;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isServer)
		{
			if (collision.transform.TryGetComponent<IDamageable>(out IDamageable damageable)) {
				damageable.TakeDamage(1);
			}

			NetworkServer.Destroy(gameObject);
		}
	}
}
