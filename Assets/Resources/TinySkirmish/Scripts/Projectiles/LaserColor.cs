using UnityEngine;
using System.Collections;

public class LaserColor : MonoBehaviour
{
    private PlayerID ThePlayerID;
    private LineRenderer TheLineRenderer;
    private ParticleSystem TheParticleSystem;
    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        TheLineRenderer = GetComponent<LineRenderer>();
        TheParticleSystem = GetComponent<ParticleSystem>();
        ThePlayerID = transform.root.GetComponent<PlayerID>();
    }

    void Update()
    {
        if (!Tracker.FullyFunctional) return;

        if(TheLineRenderer != null) TheLineRenderer.SetColors(Tracker.TheResourceManager.PlayerColor[ThePlayerID.ID].OutlineEmissionColor, Tracker.TheResourceManager.PlayerColor[ThePlayerID.ID].OutlineEmissionColor);
        if (TheParticleSystem != null) TheParticleSystem.startColor = new Color(Tracker.TheResourceManager.PlayerColor[ThePlayerID.ID].OutlineEmissionColor.r, Tracker.TheResourceManager.PlayerColor[ThePlayerID.ID].OutlineEmissionColor.g, Tracker.TheResourceManager.PlayerColor[ThePlayerID.ID].OutlineEmissionColor.b, 71 / 255.0f);
    }
}
