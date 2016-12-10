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

//Handles tracking server objects and their hierarchy, generates unique names for each object, replaces destroying
public class ObjectSyncManager : NetworkBehaviour
{
    //
    public SyncListServerGameObject ObjectsToSync = new SyncListServerGameObject();

    //When creating new object
    public void AddSyncedObject(string Name, string Parent)
    {
        ObjectsToSync.Add(new ServerGameObject(Name, Parent));
        if (isServer) RpcAddSyncedObject(Name, Parent);
    }

    //When destroying current object
    public void DestroySyncedObject()
    {

    }

    //When reassigning 
    public void ChildSyncedObject()
    {
        //Ensure to check for GetComponent<TransformSync>().IsChild
    }

    [ClientRpc]
    void RpcAddSyncedObject(string Name, string Parent)
    {
        GameObject obj = GameObject.Find(Name);
        GameObject parentObj = GameObject.Find(Parent);

        //Makes sure its childed correctly
        if (obj.transform.parent != parentObj.transform)
        {
            obj.transform.parent = parentObj.transform;
            obj.GetComponent<TransformSync>().IsChild = true;
        }
    }

    //Name Generation//////
    [SyncVar]
    public int CurrentNameNumber = 1;
    //Reusing names
    public SyncListString AvailableNames = new SyncListString();

    //Generates a name for objects
    public string GenerateUniqueName()
    {
        string retval = CurrentNameNumber.ToString();
        if (AvailableNames.Count != 0)
        {
            retval = AvailableNames[0];
            AvailableNames.RemoveAt(0);
        }
        else CurrentNameNumber += 1;
        return retval;
    }
    //Name Of Name Generation//////
}
