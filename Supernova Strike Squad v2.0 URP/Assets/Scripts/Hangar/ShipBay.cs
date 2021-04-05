using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShipBay : NetworkBehaviour
{
	[SerializeField] private Transform ownerPlatform = null;

	public Animator Animator;

	public int ownerID = -1;

	[SyncVar(hook = ("OnStateUpdate"))]
	public bool Open = false;

	public void OnStateUpdate(bool oldState, bool newState)
	{
		Animator.SetTrigger(newState ? "Open" : "Close");
	}

	private void FixedUpdate()
	{
		bool ours = ownerID == Player.LocalPlayer.ID;
		ownerPlatform.gameObject.SetActive(ours);
	}
}
