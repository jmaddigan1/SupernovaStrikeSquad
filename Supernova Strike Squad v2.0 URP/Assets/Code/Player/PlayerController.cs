using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(Rigidbody))]

public class PlayerController : NetworkBehaviour
{
	private Rigidbody rb;

	public static bool Interacting = false;

	[SerializeField] private Transform orientation;
	[SerializeField] private Transform cameraTarget = null;

    //zac slope stuff
    [SerializeField] private float slopeForce = 50f;
    [SerializeField] private float slopeForceRayLength = 1.5f;


    float jumpForce = 12f;

	float moveSpeed = 6f;
	float moveMultiplier = 10.0f;
	float airMultiplier = 0.2f;


	float groundDrag = 6f;
	float airDrag = 0.1f;

	public bool isGrounded;

	float horizontalMovement;
	float verticalMovement;

	Vector3 movementDirection;

	KeyCode jumpKey = KeyCode.Space;
    LadderClimb lClimb;

	public override void OnStartClient()
	{
        
		base.OnStartClient();

		if (hasAuthority)
		{
			FindObjectOfType<CameraController>().CameraTarget = cameraTarget;

			rb = GetComponent<Rigidbody>();
			rb.freezeRotation = true;
		}
        lClimb = GetComponent<LadderClimb>();
	}

	void Update()
	{
		if (hasAuthority)
		{
			isGrounded = Physics.Raycast(transform.position, Vector3.down, 1 + 0.1f);

			ControlDrag();
			ReadInput();

			if (Input.GetKeyDown(jumpKey) && isGrounded)
			{
				Jump();
			}
		}
	}

	void FixedUpdate()
	{
		if (hasAuthority)
		{
			if (!PlayerController.Interacting)
			{
				MovePlayer();
			}
		}
	}

	void ReadInput()
	{
		horizontalMovement = Input.GetAxisRaw("Horizontal");
		verticalMovement = Input.GetAxisRaw("Vertical");

		movementDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
	}

	void Jump()
	{
		rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
	}

	void ControlDrag()
	{
		if (rb == null) return;
	
		if (isGrounded)
		{
			rb.drag = groundDrag;
		}
		else
		{
			rb.drag = airDrag;
		}
	}

	void MovePlayer()
	{
		if (rb == null) return;

        if (lClimb.inside == true)
        {

        }
        else
        {
            if (isGrounded)
            {
                //glue
                if (OnSlope() == true)
                {
                    rb.AddForce(movementDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
                    rb.AddForce(Vector3.down + new Vector3(0, -slopeForce * 11, 0), ForceMode.Acceleration);
                }
                rb.AddForce(movementDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
            }
            else if (!isGrounded)
            {
                rb.AddForce(movementDirection.normalized * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Acceleration);
            }

            if ((verticalMovement != 0 || horizontalMovement != 0) && OnSlope())
            {

                //rb.AddForce(Vector3.down * (1 + 0.1f) * slopeForce * Time.deltaTime);
            }
        }
	}

    //zac slope stuff https://youtu.be/b7bmNDdYPzU 5:53
    private bool OnSlope()
    {
        if(!isGrounded)
        {
            return false;
        }

        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, (1 + 0.1f)* slopeForceRayLength))
        {
            if(hit.normal != Vector3.up)
            {
                return true;
            }            
        }

        return false;        
    }
}
