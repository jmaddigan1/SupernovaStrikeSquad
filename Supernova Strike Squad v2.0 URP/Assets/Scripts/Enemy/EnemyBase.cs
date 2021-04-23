using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class EnemyBase : AdvancedFSM
{
    // Delegates
    public OnEnemyDeath OnDeath;

    // Public Members
    //
    public EnemyType EnemyType;

    public Transform Target = null;
    public Rigidbody MyRigidbody = null;

    // Starting Stats
    public float MoveSpeed = 25;
    public float RotationSpeed = 1;
    public float RotationMultiplier = 1;

    public float PatrolDetectionRange = 150;
    public float PatrolPointRange = 10;
    public float AttackAngle = 10;
    public float EscapeRange = 300;

	private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, PatrolDetectionRange);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, EscapeRange);
	}

	override public IEnumerator Start()
    {
        Initialize();

        yield return null;
    }

    // Initialize the FSM
    protected override void Initialize()
    {
        // Create the FSM for the tank.
        if (isServer)
        {
            ConstructFSM();
        }
    }

    // Update each frame.
    protected override void FSMUpdate()
    {
        if (CurrentState != null && isServer)
        {
            CurrentState.Reason();
            CurrentState.Act();
        }
    }

    /// <summary>
    /// Where we add our states
    /// </summary>
    protected virtual void ConstructFSM()
    {

    }

    public List<Collider> myColliders = new List<Collider>();

	[SerializeField]public GameObject shot = null;
	public void Shoot(Transform target)
	{
        GameObject go = Instantiate(shot, transform.position + transform.forward * 3, transform.rotation);

        //      if (go.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        //      {
        //          Vector3 point = target.position + rigidbody.velocity.normalized * 2;
        //          go.transform.LookAt(point);
        //      }
        //else
        //{
        //          go.transform.LookAt(target);
        //      }

        go.transform.LookAt(target);

        // FIX COLLISION WITH OWNER
        foreach (Collider collider in myColliders)
		{
            Collider projectileColliders = go.GetComponentInChildren<Collider>();
            Collider playerCollider = collider;

            Physics.IgnoreCollision(projectileColliders, playerCollider);
        }

        NetworkServer.Spawn(go);
    }
}
