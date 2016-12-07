using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

//Replaces Network Transform and Network Transform Child with own custom interpolation
public class TransformSync : NetworkBehaviour
{
    [SyncVar]
    Vector3 realPosition = Vector3.zero;
    [SyncVar]
    Quaternion realRotation;
    //0.11f = 9 times per second
    public float SendSpeed = 0.11f;
    public float InterpolationSmoothness = 0.1f;
    private float updateInterval;

    //If this is syncing to a child object instead of the root
    public Transform ChildTarget;
    private bool IsSyncingChild = false;
    [HideInInspector]
    [SyncVar]
    public bool ChildIsDestroyed = false;

    void Start()
    {
        if (ChildTarget != null) IsSyncingChild = true;
    }

    void Update()
    {
        //Syncs destroyed child objects on server end
        if (IsSyncingChild && ChildTarget == null)
        {
            ChildIsDestroyed = true;
            Destroy(this);
            return;
        }

        if(isServer)
        {
            //Update in specific interval to prevent spamming infomation (I'm not sure if this method works for syncvar but just in case it does)
            updateInterval += Time.deltaTime;
            if(updateInterval >= SendSpeed)
            {
                //Since its already smooth on server, dont need to interpolate. Applys infomation for clients to interpolate
                if (ChildTarget == null)
                {
                    realPosition = transform.position;
                    realRotation = transform.rotation;
                }
                else
                {
                    realPosition = ChildTarget.localPosition;
                    realRotation = ChildTarget.localRotation;
                }
            }
        }
        else
        {
            //Checks client for still existing child object that was destroyed on the server and destroys it
            if(ChildIsDestroyed)
            {
                Destroy(ChildTarget.gameObject);
            }

            //Applys interpotion if on a client
            if (ChildTarget == null)
            {
                transform.position = Vector3.Lerp(transform.position, realPosition, InterpolationSmoothness);
                transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, InterpolationSmoothness);
            }
            else
            {
                ChildTarget.localPosition = Vector3.Lerp(ChildTarget.localPosition, realPosition, InterpolationSmoothness);
                ChildTarget.localRotation = Quaternion.Lerp(ChildTarget.localRotation, realRotation, InterpolationSmoothness);
            }
        }
    }
}

/*
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

//Replaces Network Transform and Network Transform Child with own custom interpolation
public class TransformSync : NetworkBehaviour
{
    public List<Transform> ObjectsToSync;
    [SyncVar]
    List<Vector3> realPositions;
    [SyncVar]
    List<Quaternion> realRotations;
    //0.11f = 9 times per second
    public float SendSpeed = 0.11f;
    public float InterpolationSmoothness = 0.1f;
    private float updateInterval;

    void Start()
    {
        ObjectsToSync.Add(transform);
        GetAllChilds(transform);
    }

    void Update()
    {
        if(isServer)
        {
            //Update in specific interval to prevent spamming infomation (I'm not sure if this method works for syncvar but just in case it does)
            updateInterval += Time.deltaTime;
            if(updateInterval >= SendSpeed)
            {
                realPositions.Clear();
                realRotations.Clear();
                foreach (Transform obj in ObjectsToSync)
                {
                    //Since its already smooth on server, dont need to interpolate. Applys infomation for clients to interpolate
                    realPositions.Add(obj.position);
                    realRotations.Add(obj.rotation);
                }
            }
        }
        else
        {
            for(int i = 0; i < ObjectsToSync.Count; ++i)
            {
                //Applys interpotion if on a client
                ObjectsToSync[i].position = Vector3.Lerp(ObjectsToSync[i].position, realPositions[i], InterpolationSmoothness);
                ObjectsToSync[i].rotation = Quaternion.Lerp(ObjectsToSync[i].rotation, realRotations[i], InterpolationSmoothness);
            }
        }
    }

    //Adds every object that desires syncing to the sync list
    void GetAllChilds(Transform root)
    {
        foreach (Transform child in root)
        {
            if(child.GetComponent<NoSync>() == null) ObjectsToSync.Add(child);
            GetAllChilds(child);
        }
    }
}
*/
