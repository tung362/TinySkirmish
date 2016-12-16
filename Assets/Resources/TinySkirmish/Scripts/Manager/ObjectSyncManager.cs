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

public class SyncListColor : SyncList<Color>
{
    protected override Color DeserializeItem(NetworkReader reader)
    {
        return reader.ReadColor();
    }

    protected override void SerializeItem(NetworkWriter writer, Color item)
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
public struct ServerColor
{
    public Color OutlineColor;
    public Color OutlineEmissionColor;
    public float OutlineEmissionlevel;
    public Color BaseColor;
    public Color BaseEmissionColor;
    public float BaseEmissionlevel;

    public ServerColor(Color NewOutlineColor, Color NewOutlineEmissionColor, float NewOutlineEmissionlevel, Color NewBaseColor, Color NewBaseEmissionColor, float NewBaseEmissionlevel)
    {
        OutlineColor = NewOutlineColor;
        OutlineEmissionColor = NewOutlineEmissionColor;
        OutlineEmissionlevel = NewOutlineEmissionlevel;
        BaseColor = NewBaseColor;
        BaseEmissionColor = NewBaseEmissionColor;
        BaseEmissionlevel = NewBaseEmissionlevel;
    }
}

public class SyncListServerColor : SyncListStruct<ServerColor> { }

[System.Serializable]
public struct ServerGameObject
{
    public string GameObjectName;
    public string GameObjectParent;
    public string GameObjectID;

    public ServerGameObject(string Name, string Parent, string ID)
    {
        GameObjectName = Name;
        GameObjectParent = Parent;
        GameObjectID = ID;
    }
}

public class SyncListServerGameObject : SyncListStruct<ServerGameObject> { }

//Handles tracking server objects and their hierarchy, generates unique names for each object, replaces destroying
public class ObjectSyncManager : NetworkBehaviour
{
    //Manager Tracker
    private bool AssignManagerTracker = true;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if (AssignManagerTracker)
        {
            FindObjectOfType<ManagerTracker>().TheObjectSyncManager = this;
            AssignManagerTracker = false;
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

    //Tracks all synced objects
    public SyncListServerGameObject ObjectsToSync = new SyncListServerGameObject();

    //When creating new object
    public void AddSyncedObject(string Name, string Parent, string ID)
    {
        if (isServer)
        {
            //Add to tracking list
            ObjectsToSync.Add(new ServerGameObject(Name, Parent, ID));

            //Syncing hierarchy to clients
            GameObject obj = GameObject.Find(Name);
            obj.GetComponent<TransformSync>().objName = Name;
            obj.GetComponent<TransformSync>().objParentName = Parent;
        }
    }

    //When destroying object
    public void DestroySyncedObject(string Name)
    {
        if (isServer)
        {
            //Finds any synced object that is childed to this object or is this object then remove from the list
            for (int i = 0; i < ObjectsToSync.Count; ++i)
            {
                if (ObjectsToSync[i].GameObjectParent == Name || ObjectsToSync[i].GameObjectName == Name)
                {
                    AvailableNames.Add(ObjectsToSync[i].GameObjectID);
                    ObjectsToSync.RemoveAt(i);
                    i -= 1;
                }
            }
            Destroy(GameObject.Find(Name));
        }
    }

    //When reassigning childs
    public void ChildSyncedObject(string Name, string Parent)
    {
        GameObject obj = GameObject.Find(Name);
        obj.GetComponent<TransformSync>().objName = Name;
        obj.GetComponent<TransformSync>().objParentName = Parent;
    }
}
