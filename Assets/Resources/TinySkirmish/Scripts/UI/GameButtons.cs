using UnityEngine;
using System.Collections;

public class GameButtons : MonoBehaviour
{
    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
    }

    public void ChangeCurrentTechView(int newValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.CurrentTechView = newValue;
    }

    public void ChangeCurrentUnitView(int newValue)
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.CurrentUnitView = newValue;
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
}
