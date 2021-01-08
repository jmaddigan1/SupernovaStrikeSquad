using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]

public class PlayerCharacterController : NetworkBehaviour
{
    private float Speed = 10.0f;

    private Rigidbody myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

	public override void OnStartAuthority()
    {
        FindObjectOfType<CameraController>().SetTarget(transform);
    }

	private void Update()
    {
        if (hasAuthority == false) return;

        float x = Input.GetAxisRaw("Horizontal");

        transform.Rotate(0, x * Speed * 10 * Time.deltaTime, 0);
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        if (hasAuthority == false) return;

        float y = Input.GetAxisRaw("Vertical");

        myRigidbody.MovePosition(myRigidbody.position + (transform.forward * y * Speed * Time.fixedDeltaTime));
    }
}
