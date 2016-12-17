using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

//Handles every command sent by the client
public class CommandManager : NetworkBehaviour
{
    //Manager Tracker
    private bool AssignManagerTracker = true;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if (!hasAuthority) return;

        if (AssignManagerTracker)
        {
            FindObjectOfType<ManagerTracker>().TheCommandManager = this;
            AssignManagerTracker = false;
        }

        //if (Input.GetMouseButtonUp(1))
        //{
        //    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

        //    foreach(RaycastHit hit in mouseHits)
        //    {
        //        if(hit.transform.tag == "Ground")
        //        {
        //            foreach (GameObject unit in TheSelectionManager.SelectedUnits)
        //            {
        //                CmdAutopath(hit.point, unit);
        //            }
        //            break;
        //        }
        //    }
        //}
    }

    //[Command]
    //void CmdAutopath(Vector3 Destination, GameObject Unit)
    //{
    //    Unit.GetComponent<ControlledUnit>().Autopath(Destination);
    //}



    //Unit Commands/////////////////////////////////////////////////////////////////
    [Command]
    public void CmdToggleAutoPath(Vector3 Destination, GameObject Unit)
    {
        Unit.GetComponent<UnitController>().ToggleAutoPath(Destination);
    }

    [Command]
    public void CmdToggleAttackBuilding(GameObject Target, GameObject Unit)
    {
        Unit.GetComponent<UnitController>().ToggleAttackBuilding(Target);
    }

    [Command]
    public void CmdToggleStayStill(GameObject Unit)
    {
        Unit.GetComponent<UnitController>().ToggleStayStill();
    }
    ////////////////////////////////////////////////////////////////////////////////

    //Lobby Commands////////////////////////////////////////////////////////////////
    [Command]
    public void CmdChangePlayerColor(Color NewOutlineColor, Color NewOutlineEmissionColor, float NewOutlineEmissionlevel, Color NewBaseColor, Color NewBaseEmissionColor, float NewBaseEmissionlevel, int ID)
    {
        FindObjectOfType<ManagerTracker>().TheResourceManager.ChangePlayerColor(NewOutlineColor, NewOutlineEmissionColor, NewOutlineEmissionlevel, NewBaseColor, NewBaseEmissionColor, NewBaseEmissionlevel, ID);
    }

    [Command]
    public void CmdChangeReadyValue(bool NewValue, int ID)
    {
        FindObjectOfType<ManagerTracker>().TheResourceManager.ChangeReadyValue(NewValue, ID);
    }

    [Command]
    public void CmdStartGame(string LevelName)
    {
        FindObjectOfType<ManagerTracker>().TheResourceManager.StartGame(LevelName);
    }
    ////////////////////////////////////////////////////////////////////////////////

    //Button Commands///////////////////////////////////////////////////////////////
    [Command]
    public void CmdChangeRapidValue(bool NewValue, int ID)
    {
        FindObjectOfType<ManagerTracker>().TheResourceManager.ChangeRapidValue(NewValue, ID);
    }

    [Command]
    public void CmdChangeMissileValue(bool NewValue, int ID)
    {
        FindObjectOfType<ManagerTracker>().TheResourceManager.ChangeMissileValue(NewValue, ID);
    }

    [Command]
    public void CmdChangeRailValue(bool NewValue, int ID)
    {
        FindObjectOfType<ManagerTracker>().TheResourceManager.ChangeRailValue(NewValue, ID);
    }

    [Command]
    public void CmdChangeLaserValue(bool NewValue, int ID)
    {
        FindObjectOfType<ManagerTracker>().TheResourceManager.ChangeLaserValue(NewValue, ID);
    }

    [Command]
    public void CmdAddToSpawnQueue(int NewValue, string TheGateName, int ClientID)
    {
        GameObject.Find(TheGateName).GetComponent<Gate>().AddToSpawnQueue(NewValue, ClientID);
    }
    ////////////////////////////////////////////////////////////////////////////////
}
