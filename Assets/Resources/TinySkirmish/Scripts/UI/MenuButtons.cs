using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuButtons : MonoBehaviour
{
    public InputField IP;
    public InputField Port;
    private NetworkManager TheNetworkManager;

    void Start()
    {
        TheNetworkManager = FindObjectOfType<NetworkManager>();
    }

    public void AppearOnClick(int ID)
    {
        AppearOnCall[] AppearOnCalls = FindObjectsOfType(typeof(AppearOnCall)) as AppearOnCall[];
        foreach (AppearOnCall objectsToAppear in AppearOnCalls)
        {
            if (objectsToAppear.ID == ID) objectsToAppear.Called = true;
        }
    }

    public void DisableAppearOnClick(int ID)
    {
        AppearOnCall[] AppearOnCalls = FindObjectsOfType(typeof(AppearOnCall)) as AppearOnCall[];
        foreach (AppearOnCall objectsToAppear in AppearOnCalls)
        {
            if (objectsToAppear.ID == ID) objectsToAppear.Called = false;
        }
    }

    public void DisableAllAppearOnClick()
    {
        AppearOnCall[] AppearOnCalls = FindObjectsOfType(typeof(AppearOnCall)) as AppearOnCall[];
        foreach (AppearOnCall objectsToAppear in AppearOnCalls) objectsToAppear.Called = false;
    }

    public void Host()
    {
        TheNetworkManager.networkPort = int.Parse(Port.text);
        TheNetworkManager.StartHost();
    }

    public void Join()
    {
        TheNetworkManager.networkAddress = IP.text;
        TheNetworkManager.networkPort = int.Parse(Port.text);
        TheNetworkManager.StartClient();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
