using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerColorChanger : MonoBehaviour
{
    public Slider OutlineRed;
    public Slider OutlineGreen;
    public Slider OutlineBlue;

    public Slider OutlineEmissionRed;
    public Slider OutlineEmissionGreen;
    public Slider OutlineEmissionBlue;
    public Slider OutlineEmissionLevel;

    public Slider BaseRed;
    public Slider BaseGreen;
    public Slider BaseBlue;

    public Slider BaseEmissionRed;
    public Slider BaseEmissionGreen;
    public Slider BaseEmissionBlue;
    public Slider BaseEmissionLevel;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
    }

    void Update()
    {
        if (!Tracker.FullyFunctional) return;
        Tracker.TheCommandManager.CmdChangePlayerColor(ConvertRGBAToColor(OutlineRed.value, OutlineGreen.value, OutlineBlue.value, 255),
                               ConvertRGBAToColor(OutlineEmissionRed.value, OutlineEmissionGreen.value, OutlineEmissionBlue.value, 255),
                                                                                                             OutlineEmissionLevel.value,
                                                                ConvertRGBAToColor(BaseRed.value, BaseGreen.value, BaseBlue.value, 255),
                                        ConvertRGBAToColor(BaseEmissionRed.value, BaseEmissionGreen.value, BaseEmissionBlue.value, 255),
                                                                                                                BaseEmissionLevel.value,
                                                                                                                            Tracker.ID);
    }

    Color ConvertRGBAToColor(float R, float G, float B, float A)
    {
        return new Color(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f);
    }
}
