using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NameSync : NetworkBehaviour
{
    [SyncVar(hook = "OnNameChange")]
    public string Name;

    void OnNameChange(string newValue)
    {
        transform.name = newValue;
    }

    void Update()
    {
        if (isServer)
        {
            if (Input.GetKeyDown(KeyCode.G)) Name = "Poop";
        }
    }
}
