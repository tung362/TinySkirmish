using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

//Replaces Network Transform and Network Transform Child with own custom interpolation
public class TransformSync : NetworkBehaviour
{
    //Only check this if its tracking the root and not the childs
    [SyncVar]
    public bool IsRoot = false;
    [SyncVar]
    public bool IsChild = false;
    [SyncVar]
    public Vector3 ServerPosition;
    [SyncVar]
    public Quaternion ServerRotation;

    //0.11f = 9 times per second
    public float SendSpeed = 0.11f;
    public float InterpolationSmoothness = 0.1f;
    private float UpdateInterval;

    //Only assign this on childs
    public Transform SyncedChildObject;
    [SyncVar]
    private bool ServerDestroyedChild = false;

    void Start()
    {
    }

    void Update()
    {
        if (isServer)
        {
            UpdateInterval += Time.deltaTime;
            if(UpdateInterval >= SendSpeed)
            {
                if (IsRoot)
                {
                    if (!IsChild)
                    {
                        ServerPosition = transform.position;
                        ServerRotation = transform.rotation;
                    }
                    else
                    {
                        ServerPosition = transform.localPosition;
                        ServerRotation = transform.localRotation;
                    }
                }
                else
                {
                    ServerPosition = SyncedChildObject.localPosition;
                    ServerRotation = SyncedChildObject.localRotation;
                }
                UpdateInterval = 0;
            }
        }
        else
        {
            if(IsRoot)
            {
                if(!IsChild)
                {
                    transform.position = Vector3.Lerp(transform.position, ServerPosition, InterpolationSmoothness);
                    transform.rotation = Quaternion.Lerp(transform.rotation, ServerRotation, InterpolationSmoothness);
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, ServerPosition, InterpolationSmoothness);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, ServerRotation, InterpolationSmoothness);
                }
            }
            else
            {
                //If the server destoryed this child then also destroy it on the client
                if(ServerDestroyedChild)
                {
                    Destroy(SyncedChildObject.gameObject);
                    Destroy(this);
                    return;
                }

                SyncedChildObject.localPosition = Vector3.Lerp(SyncedChildObject.localPosition, ServerPosition, InterpolationSmoothness);
                SyncedChildObject.localRotation = Quaternion.Lerp(SyncedChildObject.localRotation, ServerRotation, InterpolationSmoothness);
            }
        }
    }
}