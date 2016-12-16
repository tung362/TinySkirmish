using UnityEngine;
using System.Collections;

public class SpawnNetworkManager : MonoBehaviour
{
    public GameObject NetManager;

    void Awake()
    {
        if (GameObject.Find("NetworkManager") == null) Instantiate(NetManager, Vector3.zero, NetManager.transform.rotation);
    }
}