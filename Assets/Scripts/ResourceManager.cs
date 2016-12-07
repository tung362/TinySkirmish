using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Handles the client's resources
public class ResourceManager : NetworkBehaviour
{
    [SyncVar]
    public int PlayerID = 0;
    //Solars
    [SyncVar]
    public int Money = 0;
    [SyncVar]
    public int Tech = 0;
    [SyncVar]
    public int Power = 0;
}
