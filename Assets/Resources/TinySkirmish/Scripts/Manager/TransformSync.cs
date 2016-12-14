using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

//Replaces Network Transform and Network Transform Child with own custom interpolation
public class TransformSync : NetworkBehaviour
{
    //Syncing hierarchy
    [HideInInspector]
    [SyncVar]
    public string objName = "Place";
    [HideInInspector]
    [SyncVar]
    public string objParentName = "Holder";
    public bool ChildChanged = false;
    private bool RunOnce = true;
    private string PreviousParentName = "Holder";

    //Only check this if its tracking the root and not the childs
    [SyncVar]
    public bool IsRoot = false;
    [SyncVar]
    public bool IsChild = false;
    [SyncVar]
    public Vector3 ServerPosition;
    [SyncVar]
    public Quaternion ServerRotation;
    [SyncVar]
    public Vector3 ServerScale;

    //0.11f = 9 times per second
    public float SendSpeed = 0.11f;
    public float InterpolationSmoothness = 0.1f;
    private float UpdateInterval;

    //Only assign this on childs
    public Transform SyncedChildObject;

    void Start()
    {
        if (IsRoot)
        {
            if (!IsChild)
            {
                ServerPosition = transform.position;
                ServerRotation = transform.rotation;
                ServerScale = transform.localScale;
            }
            else
            {
                ServerPosition = transform.localPosition;
                ServerRotation = transform.localRotation;
                ServerScale = transform.localScale;
            }
        }
        else
        {
            ServerPosition = SyncedChildObject.localPosition;
            ServerRotation = SyncedChildObject.localRotation;
            ServerScale = SyncedChildObject.localScale;
        }
    }

    void Update()
    {
        //Checks for changes in the hierarchy and apply them
        if(PreviousParentName != objParentName)
        {
            RunOnce = true;
            PreviousParentName = objParentName;
        }

        //Waits for name sync to assign correct names on the client and then assigns the correct parent if the object exists
        if (RunOnce && IsRoot)
        {
            GameObject obj = GameObject.Find(objName);
            GameObject objParent = GameObject.Find(objParentName);
            if (objName == objParentName)
            {
                if (obj != null && objParent != null)
                {
                    obj.transform.parent = null;
                    RunOnce = false;
                }
            }
            else
            {
                if (obj != null && objParent != null)
                {
                    obj.transform.parent = objParent.transform;
                    RunOnce = false;
                }
            }
        }

        if (isServer)
        {
            //Checks if the object is childed to another object
            if (!IsChild && transform.root != transform)
            {
                IsChild = true;
                ServerPosition = transform.localPosition;
                ServerRotation = transform.localRotation;
                ServerScale = transform.localScale;
            }

            //Checks if the object was a child but went back to being the parent
            if (IsChild && transform.root == transform)
            {
                IsChild = false;
                ServerPosition = transform.position;
                ServerRotation = transform.rotation;
                ServerScale = transform.localScale;
            }

            //Updates data to clients at a specified speed rate
            UpdateInterval += Time.deltaTime;
            if(UpdateInterval >= SendSpeed)
            {
                if (IsRoot)
                {
                    if (!IsChild)
                    {
                        ServerPosition = transform.position;
                        ServerRotation = transform.rotation;
                        ServerScale = transform.localScale;
                    }
                    else
                    {
                        ServerPosition = transform.localPosition;
                        ServerRotation = transform.localRotation;
                        ServerScale = transform.localScale;
                    }
                }
                else
                {
                    ServerPosition = SyncedChildObject.localPosition;
                    ServerRotation = SyncedChildObject.localRotation;
                    ServerScale = SyncedChildObject.localScale;
                }
                UpdateInterval = 0;
            }
        }
        else
        {
            if(IsRoot)
            {
                if (!IsChild)
                {
                    transform.position = Vector3.Lerp(transform.position, ServerPosition, InterpolationSmoothness);
                    transform.rotation = Quaternion.Lerp(transform.rotation, ServerRotation, InterpolationSmoothness);
                    transform.localScale = Vector3.Lerp(transform.localScale, ServerScale, InterpolationSmoothness);
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, ServerPosition, InterpolationSmoothness);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, ServerRotation, InterpolationSmoothness);
                    transform.localScale = Vector3.Lerp(transform.localScale, ServerScale, InterpolationSmoothness);
                }
            }
            else
            {
                SyncedChildObject.localPosition = Vector3.Lerp(SyncedChildObject.localPosition, ServerPosition, InterpolationSmoothness);
                SyncedChildObject.localRotation = Quaternion.Lerp(SyncedChildObject.localRotation, ServerRotation, InterpolationSmoothness);
                SyncedChildObject.localScale = Vector3.Lerp(SyncedChildObject.localScale, ServerScale, InterpolationSmoothness);
            }
        }
    }
}