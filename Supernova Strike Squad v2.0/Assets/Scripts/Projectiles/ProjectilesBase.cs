using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class ProjectilesBase : NetworkBehaviour
{
	// Public Members
	// The forward speed of the projectile
	public float Speed = 75.0f;

	// Private Members
	private Rigidbody myRigidbody;

	// Start is called before the first frame update
	void Start()
	{
		// Get this projectiles rigidbody
		myRigidbody = GetComponent<Rigidbody>();

		// If we are NOT its owner
		// Remove this projectiles rigidbody
		if (!hasAuthority)
		{
			Destroy(myRigidbody);
			return;
		}

		// Start moving the projectile
		myRigidbody.velocity = transform.forward * Speed;

		// Set this projectiles lifetime
		Destroy(gameObject, 2.0f);
	}

	// When this projectile collides with 
	void OnCollisionEnter(Collision collision)
    {
		// Do nothing if we are NOT the projectile owner
        if (hasAuthority == false) return;

        Debug.LogError(collision.gameObject.name);
	}
}
