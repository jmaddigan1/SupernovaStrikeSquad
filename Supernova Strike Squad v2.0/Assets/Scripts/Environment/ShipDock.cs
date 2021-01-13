using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDock : MonoBehaviour
{
	public bool open = false;

	public void Open() => open = true;

	public void Close() => open = false;

	private void OnDrawGizmos()
	{
		if (open)
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.red;
		}

		Gizmos.DrawCube(transform.position, new Vector3(8, 2, 7));
	}
}
