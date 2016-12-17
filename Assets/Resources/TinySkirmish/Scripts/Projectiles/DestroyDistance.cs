using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DestroyDistance : NetworkBehaviour
{
    public float Distance = 10;
    private Vector3 StartingPosition = Vector3.zero;

    void Start()
    {
        StartingPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!isServer) return;
        if (Vector3.Distance(StartingPosition, transform.position) >= Distance) FindObjectOfType<ObjectSyncManager>().DestroySyncedObject(transform.name);
    }
}
