using UnityEngine;
using System.Collections;

public class GateSpawnSpot : MonoBehaviour
{
    public bool IsPlayer1Location = false;
    public bool IsPlayer2Location = false;
    public bool IsPlayer3Location = false;
    public bool IsPlayer4Location = false;

    public bool RunOnce = true;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
    }

    void Update()
    {
        if (!Tracker.FullyFunctional) return;
        //Places camera at correct spawn gate
        if(RunOnce)
        {
            GameObject Cam = Camera.main.gameObject;
            if (IsPlayer1Location && Tracker.ID == 0) Cam.transform.position = new Vector3(transform.position.x, Cam.transform.position.y, transform.position.z);
            else if (IsPlayer2Location && Tracker.ID == 1) Cam.transform.position = new Vector3(transform.position.x, Cam.transform.position.y, transform.position.z);
            else if (IsPlayer3Location && Tracker.ID == 2) Cam.transform.position = new Vector3(transform.position.x, Cam.transform.position.y, transform.position.z);
            else if (IsPlayer4Location && Tracker.ID == 3) Cam.transform.position = new Vector3(transform.position.x, Cam.transform.position.y, transform.position.z);
            RunOnce = false;
        }
    }
}
