using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
	[SerializeField] private Transform hitExplosion = null;

	Rigidbody rb;

	public override void OnStartServer()
	{
		Destroy(gameObject, 1.0f);
	}

	public override void OnStartClient()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rb = rigidbody;

		rb.velocity = transform.forward * 1000;
	}

	private void Start()
	{
		if (!isServer)
		{
			foreach (Collider collider in GetComponentsInChildren<Collider>()) {
				collider.enabled = false;
			}
		}
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
