using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCommand : MonoBehaviour
{
    private ManagerTracker Tracker;

	void Start ()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if (!Tracker.FullyFunctional) return;

        //Building commands
        if(Tracker.TheSelectionManager.SelectedBuildings.Count > 0)
        {

        }
        //Unit commands
        else
        {
            //Checks if anything is selected at all
            if (Tracker.TheSelectionManager.SelectedUnits.Count == 0) return;
            if (Input.GetMouseButtonUp(1))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

                //Checks
                GameObject HitBase = null;
                Vector3 groundHitPosition = Vector3.zero;

                foreach (RaycastHit hit in mouseHits)
                {
                    if (hit.transform.tag == "Building")
                    {
                        HitBase = hit.transform.gameObject;
                        return;
                    }
                    if (hit.transform.tag == "Ground") groundHitPosition = hit.point;
                }

                //Attack building
                if(HitBase != null)
                {
                    foreach(GameObject unit in Tracker.TheSelectionManager.SelectedUnits) if(unit != null) Tracker.TheCommandManager.CmdToggleAttackBuilding(HitBase, unit);
                }
                //Move to destination
                else
                {
                    //Finds a unqiue destination for each unit
                    List<Vector3> takenPositions = new List<Vector3>();
                    for(int i = 0; i < Tracker.TheSelectionManager.SelectedUnits.Count; ++i)
                    {
                        Vector3 PossiblePosition = GeneratePoint(groundHitPosition, Tracker.TheSelectionManager.SelectedUnits.Count, 0.1f);
                        if (i == 0) takenPositions.Add(PossiblePosition);
                        else
                        {
                            bool approved = true;
                            for(int j = 0; j < takenPositions.Count; ++j)
                            {
                                float distance = Vector3.Distance(PossiblePosition, takenPositions[j]);
                                if(distance < 0.1f)
                                {
                                    approved = false;
                                    break;
                                }
                            }
                            if (approved) takenPositions.Add(PossiblePosition);
                            else i -= 1;
                        }
                    }
                    for (int i = 0; i < Tracker.TheSelectionManager.SelectedUnits.Count; ++i)
                    {
                        if(Tracker.TheSelectionManager.SelectedUnits[i] != null) Tracker.TheCommandManager.CmdToggleAutoPath(takenPositions[i], Tracker.TheSelectionManager.SelectedUnits[i]);
                    }
                }
            }
        }
    }
    //num of objects * 0.1 radius of agent * 

    Vector3 GeneratePoint(Vector3 point, int Count, float AgentSize)
    {
        return point + (Random.insideUnitSphere * (Count * AgentSize * 0.4f));
    }
}
