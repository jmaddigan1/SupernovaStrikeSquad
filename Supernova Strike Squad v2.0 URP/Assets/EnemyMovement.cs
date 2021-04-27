using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : AdvancedFSM
{
    public EnemyBase Enemy = null;
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

    public List<Vector3> Points = new List<Vector3>();

	private void Awake()
	{
        BuildPatrolPoints();
    }

	override public IEnumerator Start()
    {
        yield return null;

        Initialize();

        EnvironmentSpawner environment = FindObjectOfType<EnvironmentSpawner>();
        if (environment != null && environment.CurrentEnvironment != null)
        {
            OnSpawn(environment.CurrentEnvironment);
        }
    }


    // DIRTY
    void OnSpawn(EnvironmentParameters environment)
    {
        float x = Random.Range(-1f, 1);
        float y = Random.Range(-1f, 1);
        float z = Random.Range(-1f, 1);

        Vector3 pos = Vector3.zero;

        if (environment.EnvironmentType == EnvironmentType.Sphere)
        {
            pos = new Vector3(x, 0, z).normalized * environment.EnvironmentSize.x;
        }

        if (environment.EnvironmentType == EnvironmentType.Square)
        {
            Vector3 size = environment.EnvironmentSize;
            Vector3 r = new Vector3(x, y, z);
            pos = new Vector3(r.x * size.x, r.y * size.y, r.z * size.z);
        }

        transform.position = pos;
    }
    private void BuildPatrolPoints()
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


    protected override void Initialize()
    {
        // Create the FSM for the tank.
        if (isServer)
        {
            ConstructFSM();
        }
    }

    protected override void FSMUpdate()
    {
        if (CurrentState != null && isServer)
        {
            CurrentState.Reason();

            CurrentState.Act();

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
            Quaternion neededRotation = Quaternion.LookRotation(Target.position - transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * rotSpeed);

            // Move forward
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
    }

    protected override void FSMFixedUpdate()
    {
        base.FSMFixedUpdate();
    }

    protected void ConstructFSM()
    {
        // ________________
        // PatrolState
        PatrolState patrolState = new PatrolState(new EnemyStateData(GetComponent<EnemyBase>(), this), Points);
        patrolState.AddTransition(Transition.FoundTarget, FSMStateID.TrackTarget);
        AddFSMState(patrolState);

        // ________________
        // TrackPlayerState
        TrackPlayerState rushState = new TrackPlayerState(new EnemyStateData(GetComponent<EnemyBase>(), this));
        rushState.AddTransition(Transition.LostTarget, FSMStateID.Patrol);
        rushState.AddTransition(Transition.FoundTarget, FSMStateID.ShootPlayer);
        rushState.AddTransition(Transition.ApproachedPlayer, FSMStateID.Retreating);
        AddFSMState(rushState);


        // ________________
        // ShootPlayerState
        ShootPlayerState shootPlayerState = new ShootPlayerState(new EnemyStateData(GetComponent<EnemyBase>(), this));
        shootPlayerState.AddTransition(Transition.LostTarget, FSMStateID.Patrol);
        shootPlayerState.AddTransition(Transition.ApproachedPlayer, FSMStateID.Retreating);
        AddFSMState(shootPlayerState);


        // ________________
        // ShootPlayerState
        RetreatingState retreatingState = new RetreatingState(new EnemyStateData(GetComponent<EnemyBase>(), this));
        retreatingState.AddTransition(Transition.LostTarget, FSMStateID.Patrol);
        AddFSMState(retreatingState);

    }
}
