using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LaserFire : NetworkBehaviour
{
    public LineRenderer Laser;
    public GameObject Flare;

    public float FireDelay = 0.1f;
    private float FireTimer = 0;

    public float Range = 2;

    private PlayerID ThePlayerID;
    private UnitController TheUnitController;

    void Start()
    {
        ThePlayerID = GetComponent<PlayerID>();
        TheUnitController = GetComponent<UnitController>();
        TheUnitController.MaxPursuitDistance = Range;
        FireTimer = FireDelay;
    }

    void Update()
    {
        //Find Closest Target
        GameObject target = null;
        float ClosestDistance = int.MaxValue;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range);
        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (hitColliders[i].tag == "Unit")
            {
                if (hitColliders[i].GetComponent<PlayerID>().ID != ThePlayerID.ID)
                {
                    float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                    if (distance < ClosestDistance)
                    {
                        target = hitColliders[i].gameObject;
                        ClosestDistance = distance;
                    }
                }
            }
        }

        if (isServer)
        {
            //Fire
            if (target != null)
            {
                TheUnitController.EnemyTarget = target;
                RaycastHit[] hits = Physics.RaycastAll(transform.position, (target.transform.position - transform.position).normalized);
                Laser.SetPosition(1, new Vector3(0, 0, 0));
                Flare.transform.localPosition = Vector3.zero;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.tag == "Unit")
                    {
                        float distance = Vector3.Distance(transform.position, hit.transform.position);
                        if (hit.transform.GetComponent<PlayerID>().ID != ThePlayerID.ID && distance <= Range)
                        {
                            Laser.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)));
                            Laser.transform.LookAt(target.transform);
                            Flare.transform.localPosition = new Vector3(0, 0, Vector3.Distance(transform.position, hit.point));

                            FireTimer += Time.deltaTime;
                            if(FireTimer >= FireDelay)
                            {
                                hit.transform.GetComponent<UnitStats>().Health -= 5;
                                FireTimer = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                Laser.SetPosition(1, new Vector3(0, 0, 0));
                Flare.transform.localPosition = Vector3.zero;
            }
        }
        //Client
        else
        {
            if (target != null)
            {
                RaycastHit[] hits = Physics.RaycastAll(transform.position, (target.transform.position - transform.position).normalized);
                Laser.SetPosition(1, new Vector3(0, 0, 0));
                Flare.transform.localPosition = Vector3.zero;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.tag == "Unit")
                    {
                        float distance = Vector3.Distance(transform.position, hit.transform.position);
                        if (hit.transform.GetComponent<PlayerID>().ID != ThePlayerID.ID && distance <= Range)
                        {
                            Laser.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)));
                            Laser.transform.LookAt(target.transform);
                            Flare.transform.localPosition = new Vector3(0, 0, Vector3.Distance(transform.position, hit.point));
                        }
                    }
                }
            }
            else
            {
                Laser.SetPosition(1, new Vector3(0, 0, 0));
                Flare.transform.localPosition = Vector3.zero;
            }
        }
    }
}
