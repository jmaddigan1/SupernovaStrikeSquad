using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyBase : AdvancedFSM
{    
    // Public Members
    //
    public EnemyType EnemyType;

    public Transform Target;

    override public IEnumerator Start()
    {
        Initialize();

        yield return null;
    }

    // Initialize the FSM
    protected override void Initialize()
    {
        // Create the FSM for the tank.
        ConstructFSM();
    }

    // Update each frame.
    protected override void FSMUpdate()
    {
       //  if (CurrentState != null && isServer)
        if (CurrentState != null)
        {
            CurrentState.Reason();
            CurrentState.Act();
        }
    }

    // Update each physics update.
    protected override void FSMFixedUpdate()
    {      
        if (CurrentState != null)
        {
            CurrentState.FixedReason();
            CurrentState.FixedAct();
        }
    }

    /// <summary>
    /// Where we add our states
    /// </summary>
    protected virtual void ConstructFSM()
    {

    }

    public GameObject GetTarget()
	{
        return Target.gameObject;
       // return FindObjectOfType<ShipController>().gameObject;
	}
}
