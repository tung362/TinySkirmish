using UnityEngine;
using System.Collections;

public class ManagerTracker : MonoBehaviour
{
    //Server involved
    public int ID = -1;
    public PlayerControlPanel ThePlayerControlPanel;
    public ResourceManager TheResourceManager;
    public SelectionManager TheSelectionManager;
    public CommandManager TheCommandManager;
    public ObjectSyncManager TheObjectSyncManager;

    //Client involved
    public bool FullyFunctional = false;
    public int CurrentTechView = 1;
    public int CurrentUnitView = 1;
    public bool InsufficentMoney = false;
    public bool BuildingSelected = false;

    [HideInInspector]
    public float StatbarMaxValue = 100;
    [HideInInspector]
    public float RegularShieldDamage = 35;
    [HideInInspector]
    public float RegularHullDamage = 35;
    [HideInInspector]
    public float RegularSpeed = 50;
    [HideInInspector]
    public float RegularFireRate = 50;

    [HideInInspector]
    public float RapidShieldDamage = 20;
    [HideInInspector]
    public float RapidHullDamage = 20;
    [HideInInspector]
    public float RapidSpeed = 50;
    [HideInInspector]
    public float RapidFireRate = 75;

    [HideInInspector]
    public float MissileShieldDamage = 40;
    [HideInInspector]
    public float MissileHullDamage = 50;
    [HideInInspector]
    public float MissileSpeed = 30;
    [HideInInspector]
    public float MissileFireRate = 25;

    [HideInInspector]
    public float RailShieldDamage = 5;
    [HideInInspector]
    public float RailHullDamage = 100;
    [HideInInspector]
    public float RailSpeed = 75;
    [HideInInspector]
    public float RailFireRate = 10;

    [HideInInspector]
    public float LaserShieldDamage = 100;
    [HideInInspector]
    public float LaserHullDamage = 5;
    [HideInInspector]
    public float LaserSpeed = 100;
    [HideInInspector]
    public float LaserFireRate = 100;

    void Update()
    {
        if(!FullyFunctional)
        {
            if(ID != -1 && ThePlayerControlPanel != null && TheResourceManager != null && TheSelectionManager != null && TheCommandManager != null && TheObjectSyncManager != null) FullyFunctional = true;
        }
    }
}
