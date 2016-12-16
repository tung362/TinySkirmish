using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
    [SyncVar]
    public int ID = 0;
}
