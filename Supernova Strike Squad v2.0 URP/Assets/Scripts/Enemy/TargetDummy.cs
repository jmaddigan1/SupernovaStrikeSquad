using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : EnemyBase
{
	public List<Vector3> Points = new List<Vector3>();

	private Rigidbody myRigidbody;

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();

		BuildPatrolPoints();
	}

	protected override void ConstructFSM()
	{
		//// ________________
		// PatrolState
		//PatrolState patrolState = new PatrolState(Points);
		//patrolState.AddTransition(Transition.TargetInRange, FSMStateID.Patrol);
		//AddFSMState(patrolState);


		////// ________________
		//// TrackPlayerState
		TrackPlayerState rushState = new TrackPlayerState(gameObject, GetTarget().transform);
		AddFSMState(rushState);


		////// ________________
		//// ShootPlayerState
		//ShootPlayerState shootPlayerState = new ShootPlayerState();
		//AddFSMState(shootPlayerState);
	}

	void BuildPatrolPoints()
	{
		int patrolPointsCount = 5;

		Points.Clear();
		Points.Add(transform.position);

		for (int count = 0; count < patrolPointsCount; count++)
		{
			float x = Random.Range(-1, 1f);
			float y = Random.Range(-1, 1f);
			float z = Random.Range(-1, 1f);

			Points.Add(new Vector3(x, y, z).normalized * Random.Range(0, 100));
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		myRigidbody.AddExplosionForce( 2000, collision.contacts[0].point, 5);
	}

	private void OnDrawGizmos()
	{
		foreach (Vector3 point in Points)
		{
			Gizmos.DrawCube(point, Vector3.one);
		}
	}
}