using UnityEngine;
using System.Collections;

public class GameButtons : MonoBehaviour
{
    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
    }

    public void ChangeReadyStatus(bool NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        if(Tracker.ID == 0) Tracker.TheCommandManager.CmdStartGame(Tracker.LevelName);
        else Tracker.TheCommandManager.CmdChangeReadyValue(NewValue, Tracker.ID);
    }

    public void ChangeCurrentTechView(int NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.CurrentTechView = NewValue;
    }

    public void ChangeCurrentUnitView(int NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.CurrentUnitView = NewValue;
    }

    public void ChangeRapidValue(bool NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.TheCommandManager.CmdChangeRapidValue(NewValue, Tracker.ID);
    }

    public void ChangeMissileValue(bool NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.TheCommandManager.CmdChangeMissileValue(NewValue, Tracker.ID);
    }

    public void ChangeRailValue(bool NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.TheCommandManager.CmdChangeRailValue(NewValue, Tracker.ID);
    }

    public void ChangeLaserValue(bool NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.TheCommandManager.CmdChangeLaserValue(NewValue, Tracker.ID);
    }

    public void AddToSpawnQueue(int NewValue)
    {
        if (!Tracker.FullyFunctional) return;
        if (Tracker.TheSelectionManager.SelectedBuildings.Count == 0) return;
        Tracker.TheCommandManager.CmdAddToSpawnQueue(NewValue, Tracker.TheSelectionManager.SelectedBuildings[0].gameObject.name, Tracker.ID);
    }
}
