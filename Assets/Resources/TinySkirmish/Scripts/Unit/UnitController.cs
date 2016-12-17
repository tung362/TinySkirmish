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
    [HideInInspector]
    public GameObject EnemyTarget;
    [HideInInspector]
    public Vector3 PersuitStartingPosition;
    [HideInInspector]
    public float MaxPursuitDistance = 2;
    private float SleepThreshold = 0.3f;

    private PlayerID ThePlayerID;
    private UnitStats TheUnitStats;
    private NavMeshAgent TheNavMeshAgent;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        ThePlayerID = GetComponent<PlayerID>();
        TheUnitStats = GetComponent<UnitStats>();
        TheNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    //AI
    void Update()
    {
        if (!isServer) return;
        if (!Tracker.FullyFunctional) return;

        if (AutoPath)
        {
            if (previousPosition == transform.position) AutoPath = false;
        }
        else if (AttackBuilding)
        {
            if (previousPosition == transform.position) AttackBuilding = false;
        }
        else
        {
            if(!StayStill)
            {
                //Pursuit
                if (EnemyTarget != null)
                {
                    float distance = Vector3.Distance(transform.position, EnemyTarget.transform.position);
                    if (distance < MaxPursuitDistance)
                    {
                        TheNavMeshAgent.SetDestination(EnemyTarget.transform.position);
                        SkipOnce = true;
                    }
                    else
                    {
                        SkipOnce = true;
                        EnemyTarget = null;
                    }
                }
                else
                {
                    TheNavMeshAgent.SetDestination(PersuitStartingPosition);
                    SkipOnce = true;
                }
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
        PersuitStartingPosition = Destination;
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
        PersuitStartingPosition = Target.transform.position;
        previousPosition = -transform.position;
        SkipOnce = true;
    }

    [ServerCallback]
    public void ToggleStayStill()
    {
        AutoPath = false;
        AttackBuilding = false;
        StayStill = !StayStill;
        PersuitStartingPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.tag == "Projectile")
        {
            if (other.GetComponent<PlayerID>().ID != ThePlayerID.ID)
            {
                //To do: place particles here
                if(other.GetComponent<RegularProjectile>() != null) TheUnitStats.Health -= 35;
                if (other.GetComponent<RapidProjectile>() != null) TheUnitStats.Health -= 20;
                if (other.GetComponent<MissileProjectile>() != null) TheUnitStats.Health -= 50;
                if (other.GetComponent<RailProjectile>() != null) TheUnitStats.Health -= 100;
                FindObjectOfType<ObjectSyncManager>().DestroySyncedObject(other.transform.name);
            }
        }
    }
}
