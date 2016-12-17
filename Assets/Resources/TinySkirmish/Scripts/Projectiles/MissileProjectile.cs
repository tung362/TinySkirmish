using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MissileProjectile : NetworkBehaviour
{
    public float Speed = 200;
    public float RotationSpeed = 100;
    [HideInInspector]
    public GameObject Target;

    [SyncVar]
    public Quaternion StartingRotation;

    private Rigidbody TheRigidbody;

    void Start()
    {
        TheRigidbody = GetComponent<Rigidbody>();
        transform.rotation = StartingRotation;
    }

    void FixedUpdate()
    {
        if (!isServer) return;

        //Slowly rotates towards target
        if(Target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.fixedDeltaTime);
        }
        TheRigidbody.velocity = transform.forward * Speed * Time.fixedDeltaTime;
    }
}
