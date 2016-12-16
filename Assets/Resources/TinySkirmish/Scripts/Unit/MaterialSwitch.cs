using UnityEngine;
using System.Collections;

//Handles the color of the object depending on which player it belongs to
public class MaterialSwitch : MonoBehaviour
{
    public bool IsUI = false;
    public bool IsLobby = false;
    public bool Reverse = false;

    public int LobbyID = 0;

    private Renderer TheRenderer;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        TheRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if(!Tracker.FullyFunctional) return;

        if(IsUI)
        {
            if (!Reverse)
            {
                Color outlineEmissionConverted = Tracker.TheResourceManager.PlayerColor[Tracker.ID].OutlineEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[Tracker.ID].OutlineEmissionlevel);
                TheRenderer.materials[0].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[Tracker.ID].OutlineColor);
                TheRenderer.materials[0].SetColor("_EmissionColor", outlineEmissionConverted);
                TheRenderer.materials[0].EnableKeyword("_EMISSION");
                if (TheRenderer.materials.Length > 1)
                {
                    Color baseEmissionConverted = Tracker.TheResourceManager.PlayerColor[Tracker.ID].BaseEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[Tracker.ID].BaseEmissionlevel);
                    TheRenderer.materials[1].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[Tracker.ID].BaseColor);
                    TheRenderer.materials[1].SetColor("_EmissionColor", baseEmissionConverted);
                    TheRenderer.materials[1].EnableKeyword("_EMISSION");
                }
            }
            else
            {
                Color baseEmissionConverted = Tracker.TheResourceManager.PlayerColor[Tracker.ID].BaseEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[Tracker.ID].BaseEmissionlevel);
                TheRenderer.materials[0].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[Tracker.ID].BaseColor);
                TheRenderer.materials[0].SetColor("_EmissionColor", baseEmissionConverted);
                TheRenderer.materials[0].EnableKeyword("_EMISSION");
                if (TheRenderer.materials.Length > 1)
                {
                    Color outlineEmissionConverted = Tracker.TheResourceManager.PlayerColor[Tracker.ID].OutlineEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[Tracker.ID].OutlineEmissionlevel);
                    TheRenderer.materials[1].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[Tracker.ID].OutlineColor);
                    TheRenderer.materials[1].SetColor("_EmissionColor", outlineEmissionConverted);
                    TheRenderer.materials[1].EnableKeyword("_EMISSION");
                }
            }
        }
        else if(IsLobby)
        {
            if (LobbyID > Tracker.NumberOfPlayers)
            {
                if(TheRenderer.enabled) TheRenderer.enabled = false;
                return;
            }
            if (!TheRenderer.enabled) TheRenderer.enabled = true;

            if (!Reverse)
            {
                Color outlineEmissionConverted = Tracker.TheResourceManager.PlayerColor[LobbyID].OutlineEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[LobbyID].OutlineEmissionlevel);
                TheRenderer.materials[0].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[LobbyID].OutlineColor);
                TheRenderer.materials[0].SetColor("_EmissionColor", outlineEmissionConverted);
                TheRenderer.materials[0].EnableKeyword("_EMISSION");
                if (TheRenderer.materials.Length > 1)
                {
                    Color baseEmissionConverted = Tracker.TheResourceManager.PlayerColor[LobbyID].BaseEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[LobbyID].BaseEmissionlevel);
                    TheRenderer.materials[1].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[LobbyID].BaseColor);
                    TheRenderer.materials[1].SetColor("_EmissionColor", baseEmissionConverted);
                    TheRenderer.materials[1].EnableKeyword("_EMISSION");
                }
            }
            else
            {
                Color baseEmissionConverted = Tracker.TheResourceManager.PlayerColor[LobbyID].BaseEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[LobbyID].BaseEmissionlevel);
                TheRenderer.materials[0].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[LobbyID].BaseColor);
                TheRenderer.materials[0].SetColor("_EmissionColor", baseEmissionConverted);
                TheRenderer.materials[0].EnableKeyword("_EMISSION");
                if (TheRenderer.materials.Length > 1)
                {
                    Color outlineEmissionConverted = Tracker.TheResourceManager.PlayerColor[LobbyID].OutlineEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[LobbyID].OutlineEmissionlevel);
                    TheRenderer.materials[1].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[LobbyID].OutlineColor);
                    TheRenderer.materials[1].SetColor("_EmissionColor", outlineEmissionConverted);
                    TheRenderer.materials[1].EnableKeyword("_EMISSION");
                }
            }
        }
        else
        {

        }
    }
}
