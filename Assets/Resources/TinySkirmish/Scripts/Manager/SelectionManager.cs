using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Handles the selection box
public class SelectionManager : MonoBehaviour
{
    public List<GameObject> SelectedUnits;
    public List<GameObject> SelectedBuildings;
    public Rect SelectionBox;
    private Vector2 StartingMousePosition = -Vector2.one;
    private Vector2 EndingMousePosition = -Vector2.one;
    public bool IsOverUI = false;

    //Manager Tracker
    private bool AssignManagerTracker = true;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if (AssignManagerTracker)
        {
            FindObjectOfType<ManagerTracker>().TheSelectionManager = this;
            AssignManagerTracker = false;
        }

        if (!Tracker.FullyFunctional) return;
        if (IsOverUI) IsOverUI = false;
        else
        {
            UpdateInput();
            SetRectangle();
        }
    }

    void UpdateInput()
    {
        //Start
        if (Input.GetMouseButtonDown(0))
        {
            StartingMousePosition = Input.mousePosition;
            ResetSelect();
        }

        //Held
        if (Input.GetMouseButton(0))
        {
            EndingMousePosition = Input.mousePosition;
            FakeDragSelectObject();
        }

        //Reset
        if (Input.GetMouseButtonUp(0))
        {
            StartingMousePosition = -Vector3.one;
            DragSelectObject();
            QuickSelect();
        }
    }

    //Sets the rectangle size
    void SetRectangle()
    {
        if (StartingMousePosition == -Vector2.one) return;

        float x = StartingMousePosition.x;
        float y = StartingMousePosition.y;
        float width = EndingMousePosition.x - StartingMousePosition.x;
        float height = EndingMousePosition.y - StartingMousePosition.y;

        //Flipped
        if (StartingMousePosition.x > EndingMousePosition.x)
        {
            x = EndingMousePosition.x;
            width = StartingMousePosition.x - EndingMousePosition.x;
        }
        if (StartingMousePosition.y > EndingMousePosition.y)
        {
            y = EndingMousePosition.y;
            height = StartingMousePosition.y - EndingMousePosition.y;
        }

        SelectionBox.Set(x, y, width, height);
    }

    //Hovered over
    void FakeDragSelectObject()
    {
        if (StartingMousePosition == -Vector2.one) return;

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in units)
        {
            if(unit != null)
            {
                if (SelectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)) && Tracker.ID == unit.transform.root.GetComponent<PlayerID>().ID) unit.transform.root.GetComponent<UnitStats>().IsSelected = true;
                else unit.transform.root.GetComponent<UnitStats>().IsSelected = false;
            }
        }
    }

    //Add hovered objects to list
    void DragSelectObject()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in units)
        {
            if (SelectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)) && Tracker.ID == unit.transform.root.GetComponent<PlayerID>().ID) SelectedUnits.Add(unit);
        }
    }

    //Add clicked object to list
    void QuickSelect()
    {
        if (SelectedUnits.Count == 0)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

            foreach(RaycastHit mouseHit in mouseHits)
            {
                if (mouseHit.transform.tag == "Unit")
                {
                    if (Tracker.ID == mouseHit.transform.root.GetComponent<PlayerID>().ID) SelectedUnits.Add(mouseHit.transform.root.gameObject);
                    if (mouseHit.transform != null)
                    {
                        if(Tracker.ID == mouseHit.transform.root.GetComponent<PlayerID>().ID) mouseHit.transform.root.GetComponent<UnitStats>().IsSelected = true;
                    }
                }
                if (mouseHit.transform.tag == "Gate")
                {
                    SelectedUnits.Clear();
                    SelectedBuildings.Add(mouseHit.transform.root.gameObject);
                    if (mouseHit.transform != null) mouseHit.transform.root.GetComponent<Gate>().IsSelected = true;
                    break;
                }
            }
        }
    }

    //Clears list and unapply hover effect
    void ResetSelect()
    {
        foreach (GameObject unit in SelectedUnits)
        {
            if (unit != null) unit.transform.root.GetComponent<UnitStats>().IsSelected = false;
        }
        foreach (GameObject unit in SelectedBuildings)
        {
            if (unit != null) unit.transform.root.GetComponent<Gate>().IsSelected = false;
        }
        SelectedUnits.Clear();
        SelectedBuildings.Clear();
    }
}
