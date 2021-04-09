using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
	public List<Vector3> PatrolPoints = new List<Vector3>();

	int pointIndex;

	public PatrolState(List<Vector3> points)
	{
		// Initialize this States FSMStateID
		stateID = FSMStateID.Patrol;


		// Set the patrol points this enemy has
		PatrolPoints = points;

	}

	public override void Act()
	{
	}

	public override void Reason()
	{
	}

}
