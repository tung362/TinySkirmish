using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UnitController : NetworkBehaviour
{
    //Player command auto path (overrides all other Behaviour)
    private bool AutoPath = false;
    private bool AttackBuilding = false;
    [HideInInspector]
    public GameObject TargetBuilding;
    public Vector3 previousPosition;
    public bool SkipOnce = false;

    //Agressive or defensive
    private bool StayStill = false;

    //AI
    private GameObject EnemyTarget;
    private Vector3 PersuitStartingPosition;
    private float MaxPursuitDistance = 2;
    private float SleepThreshold = 0.3f;

    private NavMeshAgent TheNavMeshAgent;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        TheNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    //AI
    void Update()
    {
        if (!isServer) return;
        if (!Tracker.FullyFunctional) return;

        if (AutoPath)
        {
            if (previousPosition == transform.position)
            {
                PersuitStartingPosition = transform.position;
                AutoPath = false;
            }
        }
        else if (AttackBuilding)
        {
            if (previousPosition == transform.position)
            {
                PersuitStartingPosition = transform.position;
                AttackBuilding = false;
            }
        }
        else
        {
            if(!StayStill)
            {
                //Pursuit
                if (EnemyTarget != null)
                {
                    float distance = Vector3.Distance(transform.position, EnemyTarget.transform.position);
                    if (distance < MaxPursuitDistance) TheNavMeshAgent.SetDestination(EnemyTarget.transform.position);
                    else
                    {
                        TheNavMeshAgent.Stop();
                        EnemyTarget = null;
                    }
                }
                else TheNavMeshAgent.SetDestination(PersuitStartingPosition);
            }
        }
        if (!SkipOnce) previousPosition = transform.position;
        else SkipOnce = false;
    }

    [ServerCallback]
    public void ToggleAutoPath(Vector3 Destination)
    {
        TargetBuilding = null;
        AttackBuilding = false;
        AutoPath = true;
        TheNavMeshAgent.SetDestination(Destination);
        previousPosition = -transform.position;
        SkipOnce = true;
    }

    [ServerCallback]
    public void ToggleAttackBuilding(GameObject Target)
    {
        AutoPath = false;
        AttackBuilding = true;
        TargetBuilding = Target;
        TheNavMeshAgent.SetDestination(Target.transform.position);
        previousPosition = -transform.position;
        SkipOnce = true;
    }

    [ServerCallback]
    public void ToggleStayStill()
    {
        AutoPath = false;
        AttackBuilding = false;
        StayStill = !StayStill;
    }
}
