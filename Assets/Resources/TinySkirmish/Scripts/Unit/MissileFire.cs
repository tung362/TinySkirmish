using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MissileFire : NetworkBehaviour
{
    public GameObject Projectile;
    public float Range = 2;
    public float FireDelay = 1.5f;
    private float FireTimer = 0;

    private PlayerID ThePlayerID;
    private UnitController TheUnitController;

    void Start()
    {
        ThePlayerID = GetComponent<PlayerID>();
        TheUnitController = GetComponent<UnitController>();
        FireTimer = FireDelay;
        TheUnitController.MaxPursuitDistance = Range;
    }

    void Update()
    {
        if (!isServer) return;

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

        //Fire
        if (target != null)
        {
            TheUnitController.EnemyTarget = target;
            FireTimer += Time.deltaTime;
            if (FireTimer >= FireDelay)
            {
                GameObject spawnedUnit = Instantiate(Projectile, transform.position, transform.rotation) as GameObject;
                //Registers for tracking
                spawnedUnit.GetComponent<MissileProjectile>().Target = target;
                spawnedUnit.GetComponent<PlayerID>().ID = ThePlayerID.ID;
                spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
                NetworkServer.Spawn(spawnedUnit);

                FireTimer = 0;
            }
        }
    }
}
