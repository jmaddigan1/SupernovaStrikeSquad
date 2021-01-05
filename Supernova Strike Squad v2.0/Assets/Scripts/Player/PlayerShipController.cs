using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShipController : NetworkBehaviour
{
  //  private Vector3 velocity;

	public override void OnStartAuthority()
    {
        Player.LocalPlayer.Ship = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 velocity = new Vector3(x, 0, y);

            transform.position += velocity * 15 * Time.deltaTime;
            // UpdateVelocity();
        }
    }


 //   [Command]
 //   public void UpdateVelocity()
	//{
 //       float x = Input.GetAxis("Horizontal");
 //       float y = Input.GetAxis("Vertical");

 //       velocity = new Vector3(x, 0, y);
 //   }
}
