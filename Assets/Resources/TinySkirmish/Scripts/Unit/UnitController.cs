using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UnitController : NetworkBehaviour
{
    //Player command auto path (overrides all other Behaviour)
    private bool AutoPath = false;
    private Vector3 Destination;

    //Agressive or defensive
    private bool StayStill = false;

    //AI
    private GameObject Target;
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
        if (!Tracker.FullyFunctional) return;

        if(AutoPath)
        {

        }
        else
        {
            if(!StayStill)
            {
                //Pursuit
                if(Target != null)
                {
                    float distance = Vector3.Distance(transform.position, Target.transform.position);
                    if (distance < MaxPursuitDistance) TheNavMeshAgent.SetDestination(Target.transform.position);
                    else Target = null;
                }
                else
                {
                    float distance = Vector3.Distance(transform.position, PersuitStartingPosition);
                    TheNavMeshAgent.SetDestination(PersuitStartingPosition);
                }
            }
        }
    }

    [ServerCallback]
    void ToggleAutoPath(Vector3 Destination)
    {

    }


}
