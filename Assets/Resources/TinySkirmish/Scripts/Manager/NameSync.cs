using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Registers synced name and tracker
public class NameSync : NetworkBehaviour
{
    [SyncVar]
    public string ObjectNameNumber;

    void Start()
    {
        //Ensures correct name on client
        if (!transform.name.Contains(ObjectNameNumber))
        {
            transform.name = transform.name + ObjectNameNumber;
            SetNamesOfChilds(transform, ObjectNameNumber);
            if(isServer) FindObjectOfType<ObjectSyncManager>().AddSyncedObject(transform.name, transform.root.name, ObjectNameNumber);
        }
    }

    //Set names for all childs that desires syncing
    void SetNamesOfChilds(Transform root, string NewValue)
    {
        foreach (Transform child in root)
        {
            child.name = child.name + NewValue;
            SetNamesOfChilds(child, NewValue);
        }
    }
}
