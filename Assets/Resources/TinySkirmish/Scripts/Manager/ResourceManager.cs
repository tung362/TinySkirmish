using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Handles the client's resources, gets updated through the server's resource manager
public class ResourceManager : NetworkBehaviour
{
    public GameObject GateBuilding;
    public bool RunOnce = true;

    //Resources
    public SyncListInt Money = new SyncListInt();

    //Tech
    public SyncListBool UnlockedRapid = new SyncListBool();
    public SyncListBool UnlockedMissile = new SyncListBool();
    public SyncListBool UnlockedRail = new SyncListBool();
    public SyncListBool UnlockedLaser = new SyncListBool();

    //Lobby
    public SyncListBool PlayerReady = new SyncListBool();

    [SyncVar(hook = "OnNumberOfPlayersChanged")]
    public int NumberOfPlayers = -1;

    //True cost for tech unlock
    [SyncVar]
    [HideInInspector]
    public int RapidCost = 200;
    [SyncVar]
    [HideInInspector]
    public int MissileCost = 300;
    [SyncVar]
    [HideInInspector]
    public int RailCost = 500;
    [SyncVar]
    [HideInInspector]
    public int LaserCost = 400;

    //Player Unit Colors
    public SyncListServerColor PlayerColor = new SyncListServerColor();

    //Manager Tracker
    private bool AssignManagerTracker = true;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if (AssignManagerTracker)
        {
            FindObjectOfType<ManagerTracker>().TheResourceManager = this;
            AssignManagerTracker = false;
        }

        if(isServer)
        {
            GameObject[] gateSpawners = GameObject.FindGameObjectsWithTag("GateSpawnSpot");

            if(gateSpawners.Length != 0)
            {
                if (RunOnce)
                {
                    foreach(GameObject gateSpawner in gateSpawners)
                    {
                        GateSpawnSpot theGateSpawnSpot = gateSpawner.GetComponent<GateSpawnSpot>();
                        if (theGateSpawnSpot.IsPlayer1Location) SpawnGate(0, new Vector3(gateSpawner.transform.position.x, -0.1f, gateSpawner.transform.position.z));
                        else if (theGateSpawnSpot.IsPlayer2Location) SpawnGate(1, new Vector3(gateSpawner.transform.position.x, -0.1f, gateSpawner.transform.position.z));
                        else if (theGateSpawnSpot.IsPlayer3Location) SpawnGate(2, new Vector3(gateSpawner.transform.position.x, -0.1f, gateSpawner.transform.position.z));
                        else if (theGateSpawnSpot.IsPlayer4Location) SpawnGate(3, new Vector3(gateSpawner.transform.position.x, -0.1f, gateSpawner.transform.position.z));
                        else SpawnGate(-1, new Vector3(gateSpawner.transform.position.x, -0.1f, gateSpawner.transform.position.z));
                    }
                    RunOnce = false;
                }
            }
        }
    }

    void OnNumberOfPlayersChanged(int NewValue)
    {
        FindObjectOfType<ManagerTracker>().NumberOfPlayers = NewValue;
    }

    Color ConvertRGBAToColor(float R, float G, float B, float A)
    {
        return new Color(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f);
    }

    [ServerCallback]
    public void AddNewPlayerResource()
    {
        Money.Add(150);
        UnlockedRapid.Add(false);
        UnlockedMissile.Add(false);
        UnlockedRail.Add(false);
        UnlockedLaser.Add(false);
        PlayerReady.Add(false);
        PlayerColor.Add(new ServerColor(ConvertRGBAToColor(0, 255, 255, 255), ConvertRGBAToColor(255, 68, 0, 255), 1, ConvertRGBAToColor(1, 1, 1, 255), ConvertRGBAToColor(1, 1, 1, 255), 0));
        NumberOfPlayers += 1;
    }

    [ServerCallback]
    public void ChangeRapidValue(bool NewValue, int ID)
    {
        if(Money[ID] >= RapidCost)
        {
            UnlockedRapid[ID] = NewValue;
            Money[ID] -= RapidCost;
        }
    }

    [ServerCallback]
    public void ChangeMissileValue(bool NewValue, int ID)
    {
        if (Money[ID] >= MissileCost)
        {
            UnlockedMissile[ID] = NewValue;
            Money[ID] -= MissileCost;
        }
    }

    [ServerCallback]
    public void ChangeRailValue(bool NewValue, int ID)
    {
        if (Money[ID] >= RailCost)
        {
            UnlockedRail[ID] = NewValue;
            Money[ID] -= RailCost;
        }
    }

    [ServerCallback]
    public void ChangeLaserValue(bool NewValue, int ID)
    {
        if (Money[ID] >= LaserCost)
        {
            UnlockedLaser[ID] = NewValue;
            Money[ID] -= LaserCost;
        }
    }

    [ServerCallback]
    public void ChangeReadyValue(bool NewValue, int ID)
    {
        PlayerReady[ID] = NewValue;
    }

    [ServerCallback]
    public void ChangePlayerColor(Color NewOutlineColor, Color NewOutlineEmissionColor, float NewOutlineEmissionlevel, Color NewBaseColor, Color NewBaseEmissionColor, float NewBaseEmissionlevel, int ID)
    {
        ServerColor modified = new ServerColor(NewOutlineColor, NewOutlineEmissionColor, NewOutlineEmissionlevel, NewBaseColor, NewBaseEmissionColor, NewBaseEmissionlevel);
        PlayerColor[ID] = modified;
    }

    [ServerCallback]
    public void StartGame(string LevelName)
    {
        bool canStart = true;
        for(int i = 1; i < PlayerReady.Count; ++i)
        {
            if (!PlayerReady[i])
            {
                canStart = false;
                break;
            }
        }
        if(canStart) FindObjectOfType<NetworkManager>().ServerChangeScene(LevelName);
    }

    public void SpawnGate(int choice, Vector3 GatePosition)
    {
        if (choice == 0)
        {
            GameObject spawnedUnit = Instantiate(GateBuilding, GatePosition, GateBuilding.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<Gate>().ID = 0;
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (choice == 1)
        {
            GameObject spawnedUnit = Instantiate(GateBuilding, GatePosition, GateBuilding.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<Gate>().ID = 1;
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (choice == 2)
        {
            GameObject spawnedUnit = Instantiate(GateBuilding, GatePosition, GateBuilding.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<Gate>().ID = 2;
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (choice == 3)
        {
            GameObject spawnedUnit = Instantiate(GateBuilding, GatePosition, GateBuilding.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<Gate>().ID = 3;
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
        else if (choice == -1)
        {
            GameObject spawnedUnit = Instantiate(GateBuilding, GatePosition, GateBuilding.transform.rotation) as GameObject;
            //Registers for tracking
            spawnedUnit.GetComponent<Gate>().ID = -1;
            spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
            NetworkServer.Spawn(spawnedUnit);
        }
    }
}
