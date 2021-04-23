using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyUtilities
{
	public static Dictionary<Directions, Vector2> GetDirectionDictionary()
	{
		// Direction Dictionary
		return new Dictionary<Directions, Vector2>()
		{
			{ Directions.North,     new Vector2( 0,  1) },
			{ Directions.NorthEast, new Vector2( 1,  1) },
			{ Directions.East,      new Vector2( 1,  0) },
			{ Directions.SouthEast, new Vector2( 1, -1) },
			{ Directions.South,     new Vector2( 0, -1) },
			{ Directions.SouthWest, new Vector2(-1, -1) },
			{ Directions.West,      new Vector2(-1,  0) },
			{ Directions.NorthWest, new Vector2(-1,  1) },
		};
	}

	// Find all the players in the scene
	public static void FindTarget(EnemyBase enemy)
	{
		enemy.Target = null;
		foreach (ShipController ship in GameObject.FindObjectsOfType<ShipController>())
		{
			// If we don't have a target, default this to the target
			if (enemy.Target == null)
			{
				enemy.Target = ship.transform;
				continue;
			}

			// Else we look for the closest player for our target
			if (Vector3.Distance(ship.transform.position, enemy.transform.position) <
				Vector3.Distance(enemy.Target.position, enemy.transform.position))
			{
				enemy.Target = ship.transform;
			}
		}
	}

	public static void Dodge(Transform self, ref Vector2 DodgeDirection, ref bool ShouldDodge, float dodgeSpeed, Vector3 raySize, float rayOffset, float rayRange)
	{
		var directionDictionary = GetDirectionDictionary();

		DodgeDirection = Vector2.zero;

		// How many of our rays hit a target
		int hitCount = 0;
		ShouldDodge = false;

		// Checking for any Obstacle in front.
		foreach (Directions directions in directionDictionary.Keys)
		{
			if (EnemyUtilities.Raycast(self.transform, directionDictionary[directions], raySize, rayOffset, rayRange))
			{
				DodgeDirection += directionDictionary[directions];
				hitCount++;
				ShouldDodge = true;
			}
		}

		if (hitCount == 8) DodgeDirection = Vector2.up;
	}

	public static bool Raycast(Transform self, Vector3 directionsOffset, Vector3 size, float offset, float range)
	{
		Vector3 pos = self.transform.position - (self.forward * 2);

		pos += self.transform.right * directionsOffset.x * offset * size.x;
		pos += self.transform.up * directionsOffset.y * offset * size.y;

		if (Physics.Raycast(pos, self.transform.TransformDirection(Vector3.forward), out RaycastHit hit, range))
		{
			Debug.DrawRay(pos, self.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
			return true;
		}
		else
		{
			Debug.DrawRay(pos, self.transform.TransformDirection(Vector3.forward) * range, Color.white);
			return false;
		}
	}

	public static float GetAngle(Transform self, Transform target)
	{
		Vector3 targetDir = target.position - self.transform.position;
		float angle = Vector3.Angle(targetDir, self.transform.forward);

		return angle;
	}

	public static float GetAngle(Transform self, Vector3 target)
	{
		Vector3 targetDir = target- self.transform.position;
		float angle = Vector3.Angle(targetDir, self.transform.forward);

		return angle;
	}

	public static float GetDistance(Transform self, Transform target)
	{
		return Vector3.Distance(self.position, target.position);
	}

	public static float GetDistance(Transform self, Vector3 target)
	{
		return Vector3.Distance(self.position, target);
	}
}
