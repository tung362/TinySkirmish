using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ControlledUnit : NetworkBehaviour
{
    public GameObject Bullet;
    public Transform ShootSpot;
    public float MaxHealth = 100;
    public float CurrentHealth = 100;

    //Server triggers
    private bool IsAutopath = false;
    private Vector3 AutopathDestination = Vector3.zero;

    private NavMeshAgent TheNavMeshAgent;

    void Start()
    {
        TheNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(isServer)
        {
            if(CurrentHealth <= 0) Die();
            if(IsAutopath) TheNavMeshAgent.SetDestination(new Vector3(AutopathDestination.x, transform.position.y, AutopathDestination.z));
        }
    }

    //Server side functions
    void ServerMove()
    {

    }

    //Destroying on the server destroys on clients also
    void Die()
    {
        Destroy(gameObject);
    }

    //Player Commands
    [ServerCallback]
    public void Move(Vector3 Direction)
    {
        transform.position += Direction * 10 * Time.deltaTime;
    }

    [ServerCallback]
    public void Shoot()
    {
        GameObject spawnedBullet = Instantiate(Bullet, ShootSpot.transform.position, Bullet.transform.rotation) as GameObject;
        spawnedBullet.GetComponent<Rigidbody>().velocity = ShootSpot.forward * 1000 * Time.deltaTime;
        NetworkServer.Spawn(spawnedBullet);
    }

    [ServerCallback]
    public void Autopath(Vector3 Destination)
    {
        IsAutopath = true;
        AutopathDestination = Destination;
    }
}
