using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]

public class PlayerCharacterController : NetworkBehaviour
{
	public Vector3 cameraOffset = new Vector3(0, 2.25f, -1.5f);

	public Vector2 InputVelocity;

	// Private Members
	private Rigidbody myRigidbody;

	#region Player Stats

	private float Speed = 10.0f;
	private float RotSpeed = 150.0f;

	#endregion


	void Start() => myRigidbody = GetComponent<Rigidbody>();

	[Command]
	public void UpdateInput(Vector2 newInput) => InputVelocity = newInput;

	#region Client

	public override void OnStartAuthority()
	{
		FindObjectOfType<CharacterCamera>().SetTarget(transform, cameraOffset);

		// Set the local connections player object to me.
		PlayerConnection.LocalPlayer.Object.PlayerObject = gameObject;
		CmdMove(PlayerConnection.LocalPlayer.playerID);
	}

	[Command]
	public void CmdMove(int id)
	{
		gameObject.transform.position = HangarLobby.Instance.SpawnPoints[id].position;

	}

	void FixedUpdate()
	{
		if (hasAuthority)
		{
			// Client sends input to server via SyncVar
			UpdateInput(new Vector2(Input.GetAxis("Vertical"), Input.GetAxisRaw("Horizontal")));
		}

		if (isServer)
		{
			// Move the player forward
			myRigidbody.MovePosition(transform.position + transform.forward * InputVelocity.x * Speed * Time.fixedDeltaTime);

			// Rotate the player
			transform.Rotate(0, InputVelocity.y * RotSpeed * Time.deltaTime, 0);
		}
	}

	bool IsGrounded()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f))
		{
			Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
			return true;
		}
		else
		{
			Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
			return false;
		}
	}

	#endregion

	#region Server

	#endregion
}
