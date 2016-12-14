using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Handles the client's resources, gets updated through the server's resource manager
public class ResourceManager : NetworkBehaviour
{
    //Resources
    public SyncListInt Money = new SyncListInt();

    //Tech
    public SyncListBool UnlockedRapid = new SyncListBool();
    public SyncListBool UnlockedMissile = new SyncListBool();
    public SyncListBool UnlockedRail = new SyncListBool();
    public SyncListBool UnlockedLaser = new SyncListBool();

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

    void Update()
    {
        if (AssignManagerTracker)
        {
            FindObjectOfType<ManagerTracker>().TheResourceManager = this;
            AssignManagerTracker = false;
        }
    }

    Color ConvertRGBAToColor(float R, float G, float B, float A)
    {
        return new Color(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f);
    }

    [ServerCallback]
    public void AddNewPlayerResource()
    {
        Money.Add(0);
        UnlockedRapid.Add(false);
        UnlockedMissile.Add(false);
        UnlockedRail.Add(false);
        UnlockedLaser.Add(false);
        PlayerColor.Add(new ServerColor(ConvertRGBAToColor(0, 255, 255, 255), ConvertRGBAToColor(255, 68, 0, 255), 1, ConvertRGBAToColor(1, 1, 1, 255), ConvertRGBAToColor(1, 1, 1, 255), 0));
    }

    [ServerCallback]
    public void ChangeRapidValue(bool NewValue, int ID)
    {
        UnlockedRapid[ID] = NewValue;
    }

    [ServerCallback]
    public void ChangeMissileValue(bool NewValue, int ID)
    {
        UnlockedMissile[ID] = NewValue;
    }

    [ServerCallback]
    public void ChangeRailValue(bool NewValue, int ID)
    {
        UnlockedRail[ID] = NewValue;
    }

    [ServerCallback]
    public void ChangeLaserValue(bool NewValue, int ID)
    {
        UnlockedLaser[ID] = NewValue;
    }

    [ServerCallback]
    public void ChangePlayerColor(Color NewOutlineColor, Color NewOutlineEmissionColor, int NewOutlineEmissionlevel, Color NewBaseColor, Color NewBaseEmissionColor, int NewBaseEmissionlevel, int ID)
    {
        ServerColor modified = new ServerColor(NewOutlineColor, NewOutlineEmissionColor, NewOutlineEmissionlevel, NewBaseColor, NewBaseEmissionColor, NewBaseEmissionlevel);
        PlayerColor[ID] = modified;
    }
}
