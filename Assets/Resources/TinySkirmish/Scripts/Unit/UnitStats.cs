using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    public bool IsSelected = false;
    public bool HasShield = false;
    public int Health = 100;
    private float HealthRegenDelay = 1;
    private int HealthRegenRate = 3;
    private float HealthTimer = 0;

    void Update()
    {
        if(isServer)
        {
            UpdateHealth();
        }
    }

    void UpdateHealth()
    {
        if(Health <= 0) Die();

        if(Health < 100)
        {
            HealthTimer += Time.deltaTime;
            if(HealthTimer >= HealthRegenDelay)
            {
                Health += HealthRegenRate;
                HealthTimer = 0;
            }
        }

        if (Health > 100) Health = 100; //Limit
        if (Health < 0) Health = 0; //Limit
    }

    void Die()
    {
        //To do: spawn particles
        FindObjectOfType<ObjectSyncManager>().DestroySyncedObject(transform.name);
    }
}
