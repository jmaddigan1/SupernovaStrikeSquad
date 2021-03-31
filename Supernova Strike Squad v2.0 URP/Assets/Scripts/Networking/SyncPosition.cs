using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Mirror;

// The "SyncPosition" was yoinked from..
// https://gamedev.stackexchange.com/questions/144933/unity-manually-sync-location-of-players-from-server-to-client

public class SyncPosition : NetworkBehaviour
{
	private GameObject playerBody;
	private Rigidbody physicsRoot;

	void Start()
	{
		playerBody = transform.GetChild(0).gameObject;
		physicsRoot = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (hasAuthority)
		{
			CmdSyncPos(transform.localPosition, transform.localRotation, physicsRoot.velocity, transform.parent.name);
		}
	}

	// Send position to the server and run the RPC for everyone, including the server. 
	[Command]
	protected void CmdSyncPos(Vector3 localPosition, Quaternion localRotation, Vector3 velocity, string parentName)
	{
		RpcSyncPos(localPosition, localRotation, velocity, parentName);
	}

	// For each player, transfer the position from the server to the client, and set it as long as it's not the local player. 
	[ClientRpc]
	void RpcSyncPos(Vector3 localPosition, Quaternion localRotation, Vector3 velocity, string parentName)
	{
		if (playerBody == null)
		{
			return;
		}
		if (!hasAuthority)
		{
			transform.localPosition = localPosition;
			transform.localRotation = localRotation;
			physicsRoot.velocity = velocity;

			if (!transform.parent.name.Equals(parentName))
			{
				transform.parent = GameObject.Find(parentName).transform;
			}
		}
	}
}
