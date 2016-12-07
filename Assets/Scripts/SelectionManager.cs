using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

//Handles the selection box
public class SelectionManager : NetworkBehaviour
{
    public List<GameObject> SelectedUnits;
    public Rect SelectionBox;
    private Vector2 StartingMousePosition = -Vector2.one;
    private Vector2 EndingMousePosition = -Vector2.one;

    void Update()
    {
        //Not sure if it actually needs to be on the server but i believe its needed to send commands also for the means of organization it is on the server
        if (hasAuthority)
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

        GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject unit in units)
        {
            //Edit later
            if(SelectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position))) unit.GetComponent<Renderer>().material.color = Color.red;
            else unit.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    //Add hovered objects to list
    void DragSelectObject()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject unit in units)
        {
            if (SelectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position))) SelectedUnits.Add(unit);
        }
    }

    //Add clicked object to list
    void QuickSelect()
    {
        if(SelectedUnits.Count == 0)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if(mouseHit.transform.tag == "Player")
                {
                    SelectedUnits.Add(mouseHit.transform.root.gameObject);
                    //Edit later
                    mouseHit.transform.root.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    //Clears list and unapply hover effect
    void ResetSelect()
    {
        foreach (GameObject unit in SelectedUnits) unit.GetComponent<Renderer>().material.color = Color.white;
        SelectedUnits.Clear();
    }
}
