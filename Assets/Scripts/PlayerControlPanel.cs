using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Handles spawning of essential managers onto the server to manage the client, player component
public class PlayerControlPanel : NetworkBehaviour
{
    public GameObject ControlledUnitPrefab;
    public GameObject ResourceManagerPrefab;
    public GameObject SelectionManagerPrefab;
    public GameObject CommandManagerPrefab;
    public GameObject SyncManagerPrefab;

    public override void OnStartLocalPlayer()
    {
        CmdInitializeServer();
    }

    //Can be called on all clients
    [Command]
    void CmdInitializeServer()
    {
        InitializeServer();
    }

    //Can only be called on server, so clients cant call on this but the server does use the client's infomation
    [ServerCallback]
    void InitializeServer()
    {
        //We want resources to be managed by the server to prevent cheating
        GameObject spawnedResourceManager = Instantiate(ResourceManagerPrefab) as GameObject;
        NetworkServer.Spawn(spawnedResourceManager);

        GameObject spawnedSelectionManager = Instantiate(SelectionManagerPrefab) as GameObject;
        NetworkServer.SpawnWithClientAuthority(spawnedSelectionManager, connectionToClient);

        if (FindObjectOfType<TransformSync>() == null)
        {
            GameObject spawnedSyncManager = Instantiate(SyncManagerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(spawnedSyncManager);
        }

        GameObject spawnedCommandManager = Instantiate(CommandManagerPrefab) as GameObject;
        spawnedCommandManager.GetComponent<CommandManager>().PlayerControlPanelObject = gameObject;
        spawnedCommandManager.GetComponent<CommandManager>().ResourceManagerObject = spawnedResourceManager;
        spawnedCommandManager.GetComponent<CommandManager>().SelectionManagerObject = spawnedSelectionManager;
        spawnedCommandManager.GetComponent<CommandManager>().TransformSyncManagerObject = FindObjectOfType<TransformSync>().gameObject;
        NetworkServer.SpawnWithClientAuthority(spawnedCommandManager, connectionToClient);

        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject spawnedUnit = Instantiate(ControlledUnitPrefab, new Vector3(0, 0.938f, 3), ControlledUnitPrefab.transform.rotation) as GameObject;
            spawnedUnit.transform.parent = FindObjectOfType<TransformSync>().transform;
            //FindObjectOfType<TransformSync>().GetComponent<TransformSync>().ObjectsToSync.Add(spawnedUnit);
            NetworkServer.Spawn(spawnedUnit);
        }
        else
        {
            GameObject spawnedUnit = Instantiate(ControlledUnitPrefab, new Vector3(0, 0.938f, 0), ControlledUnitPrefab.transform.rotation) as GameObject;
            spawnedUnit.transform.parent = FindObjectOfType<TransformSync>().transform;
            //FindObjectOfType<TransformSync>().GetComponent<TransformSync>().ObjectsToSync.Add(spawnedUnit);
            NetworkServer.Spawn(spawnedUnit);
        }
    }
}
