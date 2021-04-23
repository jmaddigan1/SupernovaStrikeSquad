using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CameraMove
{
	FollowingLocalPlayer,
	CyclingThroughScene
}

// The camera looks at  the local player.
public class CameraController : MonoBehaviour
{
	// Static Members
	public static int NO_TARGETS = -1;


	// Key Bindings
	// The key we use to switch between targets.
	public KeyCode SwitchTargetKey = KeyCode.Mouse0;


	// Public Members
	// The target we are looking at.
	public Transform CameraTarget;

	// The GameObject this camera can look at.
	public List<GameObject> Targets = new List<GameObject>();

	
	// Private Members
	// Are we looking at the local player?
	bool lookingAtPlayer = true;

	// The ship index we are looking at.
	int targetIndex;


	// Update is called once per frame
	void Update()
	{
		// If the local player is dead
		// We want to cycling through the camera targets
		if (!lookingAtPlayer)
		{
			// If we have to targets..
			if (Targets.Count == NO_TARGETS)
			{
				// TODO: Look at a default point
			}
			else
			{
				// Cycle thought the ships in the scene when were dead
				if (Input.GetKeyDown(SwitchTargetKey)) {
					IncrementTargetIndex();
				}
			}
		}

		// If we have a target
		// Look at move towards that target
		if (CameraTarget)
		{
			transform.position = CameraTarget.position;
			transform.rotation = CameraTarget.rotation;
		}
	}

	// Update the camera mode
	public void UpdateCameraMode(bool lookAtPlayer, Transform player = null)
	{
		// Are we looking at the local player?
		// Or are we cycling through camera target when were dead 
		lookingAtPlayer = lookAtPlayer;

		if (lookingAtPlayer)
		{
			CameraTarget = player;
		}
		else
		{
			FindTargets();
		}
	}

	// Find all the camera targets in the scene
	private void FindTargets()
	{
		// Clear all the current targets
		Targets.Clear();

		// Look for all the objects with the
		foreach (CameraTarget target in FindObjectsOfType<CameraTarget>())
		{
			Targets.Add(target.GetTarget());
		}

		// Set the target index
		if (Targets.Count > 0) {
			targetIndex = 0;
		}
		else
		{
			targetIndex = NO_TARGETS;
		}
	}

	// Increment the target index for the camera
	private void IncrementTargetIndex()
	{
		targetIndex = (targetIndex + 1) % Targets.Count;
		CameraTarget = Targets[targetIndex].transform;
	}

	// Force the camera to enter cycling mode
	[ContextMenu("CameraTest")]
	public void CameraTest()
	{
		UpdateCameraMode(false);
	}
}
