using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

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

public class SyncListString : SyncList<string>
{
    protected override string DeserializeItem(NetworkReader reader)
    {
        return reader.ReadString();
    }

    protected override void SerializeItem(NetworkWriter writer, string item)
    {
        writer.Write(item);
    }
}

[System.Serializable]
public struct ServerGameObject
{
    public string GameObjectName;
    public string GameObjectParent;

    public ServerGameObject(string name, string parent)
    {
        GameObjectName = name;
        GameObjectParent = parent;
    }
}

public class SyncListServerGameObject : SyncListStruct<ServerGameObject> { }

//Replaces Network Transform and Network Transform Child with own custom interpolation
//Every server game object should be a child to this object
//Every server game object must have a unique name
public class TransformSync : NetworkBehaviour
{
    public SyncListServerGameObject ObjectsToSync = new SyncListServerGameObject();
    public SyncListVector3 ServerPositions = new SyncListVector3();
    public SyncListQuaternion ServerRotations = new SyncListQuaternion();

    //Reusing names
    public SyncListString AvailableNames = new SyncListString();
    [SyncVar]
    public int currentNumber = 0;

    //0.11f = 9 times per second
    public float SendSpeed = 0.11f;
    public float InterpolationSmoothness = 0.1f;
    private float UpdateInterval;

    void Start()
    {
        ObjectsToSync.Callback = OnChangeOfObjectsToSync;
    }

    void Update()
    {
        //Server Handled
        if (isServer)
        {
            //Update in specific interval to prevent spamming infomation (I'm not sure if this method works for syncvar but just in case it does)
            UpdateInterval += Time.deltaTime;
            if (UpdateInterval >= SendSpeed)
            {
                for (int i = 0; i < ObjectsToSync.Count; ++i)
                {
                    //Since its already smooth on server, dont need to interpolate. Applys infomation for clients to interpolate
                    GameObject currentSyncedObject = GameObject.Find(ObjectsToSync[i].GameObjectName);
                    ServerPositions[i] = currentSyncedObject.transform.localPosition;
                    ServerRotations[i] = currentSyncedObject.transform.localRotation;
                }
            }
        }
        //Client Handled
        else
        {
            for (int i = 0; i < ObjectsToSync.Count; ++i)
            {
                //Applys interpotion if on a client
                GameObject currentSyncedObject = GameObject.Find(ObjectsToSync[i].GameObjectName);
                currentSyncedObject.transform.localPosition = Vector3.Lerp(currentSyncedObject.transform.localPosition, ServerPositions[i], InterpolationSmoothness);
                currentSyncedObject.transform.localRotation = Quaternion.Lerp(currentSyncedObject.transform.localRotation, ServerRotations[i], InterpolationSmoothness);
            }
        }
    }

    //Syncs the changes, assuming client and server names are the same
    void OnChangeOfObjectsToSync(SyncListServerGameObject.Operation op, int itemIndex)
    {
        //Set Child
        GameObject currentSyncedObject = GameObject.Find(ObjectsToSync[itemIndex].GameObjectName);
        GameObject currentSyncedParent = GameObject.Find(ObjectsToSync[itemIndex].GameObjectParent);

        //Newly added
        if (currentSyncedObject != null)
        {
            //Checks if the root is the transform sync manager by default, if not then set it to be
            if (currentSyncedParent.transform.root != transform) currentSyncedObject.transform.parent = transform;

            //Checks if theres any child objects attached to this object that desires syncing
            GetAllChilds(currentSyncedObject.transform);

            //Syncs the hierarchy
            if (currentSyncedObject.transform.parent != currentSyncedParent.transform) currentSyncedObject.transform.parent = currentSyncedParent.transform;
        }
        //Destroyed
        else
        {
            ObjectsToSync.RemoveAt(itemIndex);
        }

        Debug.Log(itemIndex);
    }

    void GetAllChilds(Transform root)
     {
         foreach (Transform child in root)
         {
            if (child.GetComponent<NoSync>() == null)
            {
                ServerGameObject SyncedChild = new ServerGameObject();
                SyncedChild.GameObjectName = child.name;
                SyncedChild.GameObjectParent = root.name;
                ObjectsToSync.Add(SyncedChild);
            }
         }
     }

    //public string NameGenerator(Transform obj)
    //{

    //}
}