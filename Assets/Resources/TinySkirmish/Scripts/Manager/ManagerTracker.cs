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

    public float StatbarMaxValue = 100;
    public float RegularShieldDamage = 35;
    public float RegularHullDamage = 35;
    public float RegularSpeed = 50;
    public float RegularFireRate = 50;

    public float RapidShieldDamage = 20;
    public float RapidHullDamage = 20;
    public float RapidSpeed = 50;
    public float RapidFireRate = 75;

    public float MissileShieldDamage = 40;
    public float MissileHullDamage = 50;
    public float MissileSpeed = 30;
    public float MissileFireRate = 25;

    public float RailShieldDamage = 5;
    public float RailHullDamage = 100;
    public float RailSpeed = 75;
    public float RailFireRate = 10;

    public float LaserShieldDamage = 100;
    public float LaserHullDamage = 5;
    public float LaserSpeed = 100;
    public float LaserFireRate = 100;

    void Update()
    {
        if(!FullyFunctional)
        {
            if(ID != -1 && ThePlayerControlPanel != null && TheResourceManager != null && TheSelectionManager != null && TheCommandManager != null && TheObjectSyncManager != null) FullyFunctional = true;
        }
    }
}
