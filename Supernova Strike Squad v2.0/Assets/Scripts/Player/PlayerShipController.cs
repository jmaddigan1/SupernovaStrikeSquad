using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShipController : NetworkBehaviour
{
	//  private Vector3 velocity;

	public float Speed = 15.0f;

	private void Start()
	{

		DontDestroyOnLoad(gameObject);
	}

	public override void OnStartAuthority()
	{
		FindObjectOfType<CameraController>().SetTarget(transform);

	}

	// Update is called once per frame
	void Update()
	{
		if (hasAuthority)
		{
			transform.position += transform.forward * Speed * Time.deltaTime; 

			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			//Vector3 velocity = new Vector3(0, 0, y);

			//transform.position += velocity * 15 * Time.deltaTime;

			transform.Rotate(-y * 45 * Time.deltaTime, x * 45 * Time.deltaTime, 0);
			// UpdateVelocity();
		}
	}

	public int ownerID;


	//   [Command]
	//   public void UpdateVelocity()
	//{
	//       float x = Input.GetAxis("Horizontal");
	//       float y = Input.GetAxis("Vertical");

	//       velocity = new Vector3(x, 0, y);
	//   }
}
