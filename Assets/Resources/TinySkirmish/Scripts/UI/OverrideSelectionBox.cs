using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class OverrideSelectionBox : MonoBehaviour
{
    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
    }

	void Update ()
    {
        if (!Tracker.FullyFunctional) return;

        if (EventSystem.current.IsPointerOverGameObject()) Tracker.TheSelectionManager.IsOverUI = true;
    }
}
