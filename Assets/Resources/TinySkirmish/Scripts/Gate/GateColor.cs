using UnityEngine;
using System.Collections;

public class GateColor : MonoBehaviour
{
    private Renderer TheRenderer;
    private Gate TheGate;
    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        TheRenderer = GetComponent<Renderer>();
        TheGate = GetComponent<Gate>();
    }

    void Update()
    {
        if (!Tracker.FullyFunctional) return;
        if(TheGate.ID != -1)
        {
            Color outlineEmissionConverted = Tracker.TheResourceManager.PlayerColor[TheGate.ID].OutlineEmissionColor * Mathf.LinearToGammaSpace(Tracker.TheResourceManager.PlayerColor[TheGate.ID].OutlineEmissionlevel);
            TheRenderer.materials[0].SetColor("_Color", Tracker.TheResourceManager.PlayerColor[TheGate.ID].OutlineColor);
            TheRenderer.materials[0].SetColor("_EmissionColor", outlineEmissionConverted);
            TheRenderer.materials[0].EnableKeyword("_EMISSION");
        }
        else
        {
            TheRenderer.materials[0].SetColor("_Color", Color.white);
            TheRenderer.materials[0].SetColor("_EmissionColor", Color.black);
            TheRenderer.materials[0].EnableKeyword("_EMISSION");
        }
    }
}
