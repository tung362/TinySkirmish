using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DestroyTimer : NetworkBehaviour
{
    private float Timer = 0;
    public float Delay = 4;

    void FixedUpdate()
    {
        if (!isServer) return;
        Timer += Time.fixedDeltaTime;
        if (Timer >= Delay) FindObjectOfType<ObjectSyncManager>().DestroySyncedObject(transform.name);
    }
}
