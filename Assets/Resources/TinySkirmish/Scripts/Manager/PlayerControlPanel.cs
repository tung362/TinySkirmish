using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Handles spawning of essential managers onto the server to manage the client, player component
public class PlayerControlPanel : NetworkBehaviour
{
    public GameObject ResourceManagerPrefab;
    public GameObject CommandManagerPrefab;
    public GameObject ObjectSyncManagerPrefab;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public override void OnStartLocalPlayer()
    {
        //Server Setup
        if (isServer) InitializeServer();
        //Client Setup
        CmdInitializeClient();

        FindObjectOfType<ManagerTracker>().ThePlayerControlPanel = this;
        FindObjectOfType<ManagerTracker>().ID = GameObject.FindGameObjectsWithTag("Player").Length - 1;
    }

    [ServerCallback]
    void InitializeServer()
    {
        //Spawns essential managers
        if (FindObjectOfType<ResourceManager>() == null)
        {
            GameObject spawnedServerResourceManager = Instantiate(ResourceManagerPrefab) as GameObject;
            NetworkServer.Spawn(spawnedServerResourceManager);
        }

        if (FindObjectOfType<ObjectSyncManager>() == null)
        {
            GameObject spawnedSyncManager = Instantiate(ObjectSyncManagerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(spawnedSyncManager);
        }

        //GameObject spawnedUnit = Instantiate(ControlledUnitPrefab, new Vector3(0, 0, 0), ControlledUnitPrefab.transform.rotation) as GameObject;
        ////Registers for tracking
        //spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
        //NetworkServer.Spawn(spawnedUnit);
    }

    [Command]
    void CmdInitializeClient()
    {
        FindObjectOfType<ResourceManager>().AddNewPlayerResource();

        GameObject spawnedCommandManager = Instantiate(CommandManagerPrefab) as GameObject;
        NetworkServer.SpawnWithClientAuthority(spawnedCommandManager, connectionToClient);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) Test();
        if (Input.GetKeyDown(KeyCode.H)) Test2();
    }

    public GameObject TestObj;

    [ServerCallback]
    void Test()
    {
        GameObject spawnedUnit = Instantiate(TestObj, new Vector3(0, 0, 0), TestObj.transform.rotation) as GameObject;
        //Registers for tracking
        spawnedUnit.GetComponent<PlayerID>().ID = 0;
        spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
        NetworkServer.Spawn(spawnedUnit);
    }

    [ServerCallback]
    void Test2()
    {
        GameObject spawnedUnit = Instantiate(TestObj, new Vector3(2.58f, 0, 1.4f), TestObj.transform.rotation) as GameObject;
        //Registers for tracking
        spawnedUnit.GetComponent<PlayerID>().ID = 1;
        spawnedUnit.GetComponent<NameSync>().ObjectNameNumber = FindObjectOfType<ObjectSyncManager>().GenerateUniqueName();
        NetworkServer.Spawn(spawnedUnit);
    }
}
