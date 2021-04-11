using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class EnemyBase : AdvancedFSM
{    
    // Public Members
    //
    public EnemyType EnemyType;

    public Transform Target = null;
    public Rigidbody MyRigidbody = null;

    // Starting Stats
    public float MoveSpeed = 25;
    public float RotationSpeed = 1;
    public float RotationMultiplier = 1;

    public float PatrolDetectionRange = 300;
    public float PatrolPointRange = 10;
    public float AttackAngle = 10;
    public float EscapeRange = 350;

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

	[SerializeField] GameObject shot = null;
	public void Shoot()
	{
        GameObject go = Instantiate(shot, transform.position + transform.forward * 3, transform.rotation);
        NetworkServer.Spawn(go);
    }
}
