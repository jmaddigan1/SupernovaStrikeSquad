using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : EnemyBase
{
	public List<Vector3> Points = new List<Vector3>();

	private void Awake()
	{
		MyRigidbody = GetComponent<Rigidbody>();

		BuildPatrolPoints();
	}

	protected override void FSMFixedUpdate()
	{
		base.FSMFixedUpdate();

		// If we don't have a target, do nothing
		if (Target == null) return;

		// If the target is NOT in front of us
		if (EnemyUtilities.GetAngle(transform, Target) > 1)
		{
			// We want to slowly increase out rotation speed multiplier
			RotationMultiplier = Mathf.Lerp(RotationMultiplier, 2.5f, Time.deltaTime * 0.5f);
		}
		else
		{
			// Else we are looking at the target
			RotationMultiplier = Mathf.Lerp(RotationMultiplier, 1f, Time.deltaTime * 0.5f);
		}

		// Look at the target
		float rotSpeed = RotationSpeed * RotationMultiplier;
		Quaternion neededRotation = Quaternion.LookRotation(Target.position - transform.position,transform.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * rotSpeed);

		// Move forward
		transform.position += transform.forward * MoveSpeed * Time.deltaTime;
	}

	protected override void ConstructFSM()
	{
		// ________________
		// PatrolState
		PatrolState patrolState = new PatrolState(this, Points);
		patrolState.AddTransition(Transition.FoundTarget, FSMStateID.TrackTarget);
		AddFSMState(patrolState);
		
		// ________________
		// TrackPlayerState
		TrackPlayerState rushState = new TrackPlayerState(this, new TrackPlayerStateData());
		rushState.AddTransition(Transition.LostTarget, FSMStateID.Patrol);
		rushState.AddTransition(Transition.FoundTarget, FSMStateID.ShootPlayer);
		rushState.AddTransition(Transition.ApproachedPlayer, FSMStateID.Retreating);
		AddFSMState(rushState);


		//// ________________
		// ShootPlayerState
		ShootPlayerState shootPlayerState = new ShootPlayerState(this, new ShootPlayerStateData());
		shootPlayerState.AddTransition(Transition.LostTarget, FSMStateID.Patrol);
		shootPlayerState.AddTransition(Transition.ApproachedPlayer, FSMStateID.Retreating);
		AddFSMState(shootPlayerState);


		//// ________________
		// ShootPlayerState
		RetreatingState retreatingState = new RetreatingState(this);
		retreatingState.AddTransition(Transition.LostTarget, FSMStateID.Patrol);
		AddFSMState(retreatingState);

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

	private void OnDrawGizmos()
	{
		foreach (Vector3 point in Points)
		{
			Gizmos.DrawCube(point, Vector3.one);
		}

		if (Target)
		{
			Gizmos.DrawLine(transform.position, Target.position);
		}
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1, 0, 0, 0.2f);
		Gizmos.DrawSphere(transform.position, PatrolDetectionRange);

		//Gizmos.color = new Color(0, 1, 0, 0.2f);
		//Gizmos.DrawWireSphere(transform.position, PatrolEscapeRange);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//MyRigidbody.AddExplosionForce( 2000, collision.contacts[0].point, 5);
	}
}