using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class SyncListGameObject : SyncList<GameObject>
{
    protected override GameObject DeserializeItem(NetworkReader reader)
    {
        return reader.ReadGameObject();
    }

    protected override void SerializeItem(NetworkWriter writer, GameObject item)
    {
        writer.Write(item);
    }
}

public class SyncListVector3 : SyncList<Vector3>
{
    protected override Vector3 DeserializeItem(NetworkReader reader)
    {
        return reader.ReadVector3();
    }

    protected override void SerializeItem(NetworkWriter writer, Vector3 item)
    {
        writer.Write(item);
    }
}

public class SyncListQuaternion : SyncList<Quaternion>
{
    protected override Quaternion DeserializeItem(NetworkReader reader)
    {
        return reader.ReadQuaternion();
    }

    protected override void SerializeItem(NetworkWriter writer, Quaternion item)
    {
        writer.Write(item);
    }
}

//Replaces Network Transform and Network Transform Child with own custom interpolation
public class TransformSync : NetworkBehaviour
{
    public SyncListGameObject ObjectsToSync = new SyncListGameObject();
    public SyncListVector3 RealPositions = new SyncListVector3();
    public SyncListQuaternion RealRotations = new SyncListQuaternion();

    public List<Transform> LocalObjects = new List<Transform>();

    //0.11f = 9 times per second
    public float SendSpeed = 0.11f;
    public float InterpolationSmoothness = 0.1f;
    private float UpdateInterval;

    void Start()
    {
        ObjectsToSync.Callback = OnChangeOfObjectsToSync;
        RealPositions.Callback = OnChangeOfPositions;
    }

    void Update()
    {
        //Server Handled
        if (isServer)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                RealPositions.Add(new Vector3(0, 0, 0));
            }

            //Update in specific interval to prevent spamming infomation (I'm not sure if this method works for syncvar but just in case it does)
            //UpdateInterval += Time.deltaTime;
            //if (UpdateInterval >= SendSpeed)
            //{
            //    for (int i = 0; i < ObjectsToSync.Count; ++i)
            //    {
            //        //Since its already smooth on server, dont need to interpolate. Applys infomation for clients to interpolate
            //        RealPositions[i] = ObjectsToSync[i].transform.localPosition;
            //        RealRotations[i] = ObjectsToSync[i].transform.localRotation;
            //    }
            //}
        }
        //Client Handled
        else
        {
            //for (int i = 0; i < ObjectsToSync.Count; ++i)
            //{
            //    //Applys interpotion if on a client
            //    ObjectsToSync[i].transform.localPosition = Vector3.Lerp(ObjectsToSync[i].transform.localPosition, RealPositions[i], InterpolationSmoothness);
            //    ObjectsToSync[i].transform.localRotation = Quaternion.Lerp(ObjectsToSync[i].transform.localRotation, RealRotations[i], InterpolationSmoothness);
            //}
        }
    }

    //Checks for any deleted objects on the server and destroy them on the client
    void OnChangeOfObjectsToSync(SyncListGameObject.Operation op, int itemIndex)
    {
        //Debug.Log("Augoo");
        //Compares client list with serverlist and deletes client objects that dont exist on the server;
        //for (int i = 0; i < LocalObjects.Count; ++i)
        //{
        //    bool HasMatched = false;
        //    for(int j = 0; j < ObjectsToSync.Count; ++j)
        //    {
        //        if (LocalObjects[i] == ObjectsToSync[j])
        //        {
        //            HasMatched = true;
        //            break;
        //        }
        //    }

        //    if (!HasMatched)
        //    {
        //        Destroy(LocalObjects[i].gameObject);
        //        LocalObjects.RemoveAt(i);
        //    }
        //}
    }

    void OnChangeOfPositions(SyncListVector3.Operation op, int itemIndex)
    {
        Debug.Log("Boop");
    }

    //Adds every object that desires syncing to the sync list
    void GetAllChilds(Transform root)
    {
        foreach (Transform child in root)
        {
            if (child.GetComponent<NoSync>() == null) ObjectsToSync.Add(child.gameObject);
            GetAllChilds(child);
        }
    }
}