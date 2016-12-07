using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

//Handles every command sent by the client
public class CommandManager : NetworkBehaviour
{
    //Local manager
    [SyncVar]
    public GameObject PlayerControlPanelObject;
    [SyncVar]
    public GameObject ResourceManagerObject;
    [SyncVar]
    public GameObject SelectionManagerObject;

    private PlayerControlPanel ThePlayerControlPanel;
    private ResourceManager TheResourceManager;
    private SelectionManager TheSelectionManager;

    void Start()
    {
        ThePlayerControlPanel = PlayerControlPanelObject.GetComponent<PlayerControlPanel>();
        TheResourceManager = ResourceManagerObject.GetComponent<ResourceManager>();
        TheSelectionManager = SelectionManagerObject.GetComponent<SelectionManager>();
    }

    void Update()
    {
        if (!hasAuthority) return;

        if(Input.GetMouseButtonUp(1))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

            foreach(RaycastHit hit in mouseHits)
            {
                if(hit.transform.tag == "Ground")
                {
                    //CmdAutopath(hit.point);
                    foreach (GameObject unit in TheSelectionManager.SelectedUnits)
                    {
                        CmdAutopath(hit.point, unit);
                    }
                    break;
                }
            }
        }
    }

    [Command]
    void CmdAutopath(Vector3 Destination, GameObject Unit)
    {
        Unit.GetComponent<ControlledUnit>().Autopath(Destination);
    }
}
