using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gate : NetworkBehaviour
{
    [SyncVar]
    public int ID = -1;

    //The attacker
    public int AttackerID = 0;

    public float GenMoneyDelay = 1;
    public int GenMoneyRate = 5;
    private float GenMoneyTimer = 0;

    public float Health = 100;
    public float RegenHealthDelay = 1;
    public float RegenHealthRate = 5;
    private float HealthTimer = 0;

    public float SpawnDelay = 0.5f;
    private float SpawnTimer = 0;
    public SyncListInt SpawnQueue = new SyncListInt();

    public bool IsSelected = false;

    public GameObject RegularUnit;
    public GameObject RapidUnit;
    public GameObject MissileUnit;
    public GameObject RailUnit;
    public GameObject LaserUnit;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
    }

    void Update()
    {
        if (!isServer) return;
        if(Tracker.TheResourceManager.NumberOfPlayers < ID) ID = -1;
        UpdateHealth();
        GenerateMoney();
        SpawnUnits();
    }

    void UpdateHealth()
    {
        if (Health <= 0) Die();

        //Dont regen if its neutral
        if (ID != -1)
        {
            if (Health < 100)
            {
                HealthTimer += Time.deltaTime;
                if (HealthTimer >= RegenHealthDelay)
                {
                    Health += RegenHealthRate;
                    HealthTimer = 0;
                }
            }
        }

        if (Health > 100) Health = 100; //Limit
        if (Health < 0) Health = 0; //Limit
    }

    void Die()
    {
        //Return to neutral
        if (ID != -1)
        {
            SpawnQueue.Clear();
            Health = 100;
            ID = -1;
            return;
        }
        //Give ownership to attacker
        else
        {
            SpawnQueue.Clear();
            Health = 100;
            ID = AttackerID;
            return;
        }
    }

    void GenerateMoney()
    {
        if(ID != -1)
        {
            GenMoneyTimer += Time.deltaTime;
            if (GenMoneyTimer >= GenMoneyDelay)
            {
                Tracker.TheResourceManager.Money[ID] += GenMoneyRate;
                GenMoneyTimer = 0;
            }
        }
    }

    void SpawnUnits()
    {
        if (ID == -1) return;
        if (SpawnQueue.Count != 0)
        {
            SpawnTimer += Time.deltaTime;
            if (SpawnTimer >= SpawnDelay)
            {
                SpawnAUnit(SpawnQueue[0], ID);
                SpawnTimer = 0;
            }
        }
    }

    Vector3 GeneratePoint(Vector3 point)
    {
        return point + (Random.insideUnitSphere * 1);
    }

    [ServerCallback]
    void SpawnAUnit(int UnitType, int UnitID)
    {
        if(UnitType == 0)
        {
            GameObject spawnedUnit = Instantiate(RegularUnit, new Vector3(transform.position.x, 0, transform.position.z), RegularUnit.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<PlayerID>().ID = UnitID;
            spawnedUnit.GetComponent<UnitController>().PersuitStartingPosition = GeneratePoint(new Vector3(transform.position.x, 0, transform.position.z));
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (UnitType == 1)
        {
            GameObject spawnedUnit = Instantiate(RapidUnit, new Vector3(transform.position.x, 0, transform.position.z), RapidUnit.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<PlayerID>().ID = UnitID;
            spawnedUnit.GetComponent<UnitController>().PersuitStartingPosition = GeneratePoint(new Vector3(transform.position.x, 0, transform.position.z));
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (UnitType == 2)
        {
            GameObject spawnedUnit = Instantiate(MissileUnit, new Vector3(transform.position.x, 0, transform.position.z), MissileUnit.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<PlayerID>().ID = UnitID;
            spawnedUnit.GetComponent<UnitController>().PersuitStartingPosition = GeneratePoint(new Vector3(transform.position.x, 0, transform.position.z));
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (UnitType == 3)
        {
            GameObject spawnedUnit = Instantiate(RailUnit, new Vector3(transform.position.x, 0, transform.position.z), RailUnit.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<PlayerID>().ID = UnitID;
            spawnedUnit.GetComponent<UnitController>().PersuitStartingPosition = GeneratePoint(new Vector3(transform.position.x, 0, transform.position.z));
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (UnitType == 4)
        {
            GameObject spawnedUnit = Instantiate(LaserUnit, new Vector3(transform.position.x, 0, transform.position.z), LaserUnit.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<PlayerID>().ID = UnitID;
            spawnedUnit.GetComponent<UnitController>().PersuitStartingPosition = GeneratePoint(new Vector3(transform.position.x, 0, transform.position.z));
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        SpawnQueue.RemoveAt(0);
    }

    //Commands
    [ServerCallback]
    public void AddToSpawnQueue(int NewValue, int ClientID)
    {
        if(ID == -1) return;
        if(ClientID == ID)
        {
            //Prices
            if(NewValue == 0)
            {
                if (Tracker.TheResourceManager.Money[ID] >= 10)
                {
                    SpawnQueue.Add(NewValue);
                    Tracker.TheResourceManager.Money[ID] -= 10;
                }
            }
            //Prices
            else if (NewValue == 1)
            {
                if (Tracker.TheResourceManager.Money[ID] >= 15 && Tracker.TheResourceManager.UnlockedRapid[ID])
                {
                    SpawnQueue.Add(NewValue);
                    Tracker.TheResourceManager.Money[ID] -= 15;
                }
            }
            //Prices
            else if (NewValue == 2)
            {
                if (Tracker.TheResourceManager.Money[ID] >= 20 && Tracker.TheResourceManager.UnlockedMissile[ID])
                {
                    SpawnQueue.Add(NewValue);
                    Tracker.TheResourceManager.Money[ID] -= 20;
                }
            }
            //Prices
            else if (NewValue == 3)
            {
                if (Tracker.TheResourceManager.Money[ID] >= 30 && Tracker.TheResourceManager.UnlockedRail[ID])
                {
                    SpawnQueue.Add(NewValue);
                    Tracker.TheResourceManager.Money[ID] -= 30;
                }
            }
            //Prices
            else if (NewValue == 4)
            {
                if (Tracker.TheResourceManager.Money[ID] >= 25 && Tracker.TheResourceManager.UnlockedLaser[ID])
                {
                    SpawnQueue.Add(NewValue);
                    Tracker.TheResourceManager.Money[ID] -= 25;
                }
            }
        }
    }
} 