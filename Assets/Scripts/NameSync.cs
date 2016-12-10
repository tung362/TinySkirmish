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
        //Ensures correct name
        if (!transform.name.Contains(ObjectNameNumber))
        {
            transform.name = transform.name + ObjectNameNumber;
            SetNamesOfChilds(transform, ObjectNameNumber);
            if(isServer) FindObjectOfType<ObjectSyncManager>().AddSyncedObject(transform.name, "Unit(Clone)1");
        }
    }

    //Set names for all childs that desires syncing
    void SetNamesOfChilds(Transform root, string NewValue)
    {
        foreach (Transform child in root)
        {
            if (child.GetComponent<NoSync>() == null) child.name = child.name + NewValue;
            SetNamesOfChilds(child, NewValue);
        }
    }
}
