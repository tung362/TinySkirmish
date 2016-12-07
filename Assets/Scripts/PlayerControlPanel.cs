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
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject spawnedUnit = Instantiate(ControlledUnitPrefab, new Vector3(0, 0.938f, 3), ControlledUnitPrefab.transform.rotation) as GameObject;
            NetworkServer.Spawn(spawnedUnit);
        }
        else
        {
            GameObject spawnedUnit = Instantiate(ControlledUnitPrefab, new Vector3(0, 0.938f, 0), ControlledUnitPrefab.transform.rotation) as GameObject;
            NetworkServer.Spawn(spawnedUnit);
        }

        //We want resources to be managed by the server to prevent cheating
        GameObject spawnedResourceManager = Instantiate(ResourceManagerPrefab) as GameObject;
        //spawnedResourceManager.name = "Player " + connectionToClient.connectionId + " Resource Manager";
        NetworkServer.Spawn(spawnedResourceManager);

        GameObject spawnedSelectionManager = Instantiate(SelectionManagerPrefab) as GameObject;
        //spawnedSelectionManager.name = "Player " + connectionToClient.connectionId + " Selection Manager";
        NetworkServer.SpawnWithClientAuthority(spawnedSelectionManager, connectionToClient);

        GameObject spawnedCommandManager = Instantiate(CommandManagerPrefab) as GameObject;
        spawnedCommandManager.GetComponent<CommandManager>().PlayerControlPanelObject = gameObject;
        spawnedCommandManager.GetComponent<CommandManager>().ResourceManagerObject = spawnedResourceManager;
        spawnedCommandManager.GetComponent<CommandManager>().SelectionManagerObject = spawnedSelectionManager;
        //spawnedCommandManager.name = "Player " + connectionToClient.connectionId + " Command Manager";
        NetworkServer.SpawnWithClientAuthority(spawnedCommandManager, connectionToClient);
    }
}
