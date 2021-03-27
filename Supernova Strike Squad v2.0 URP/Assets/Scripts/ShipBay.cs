using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShipBay : NetworkBehaviour
{
	public int ownerID = -1;

	public Animator Animator;

	[SyncVar(hook = ("OnStateUpdate"))]
	public bool Open = false;

	public void OnStateUpdate(bool oldState, bool newState)
	{
		Animator.SetTrigger(Open ? "Open" : "Close");
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Open ? Color.green : Color.red;
		Gizmos.DrawCube(transform.position + Vector3.up * 5, Vector3.one * 2);
	}
}
