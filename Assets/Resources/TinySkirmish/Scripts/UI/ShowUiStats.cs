using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowUiStats : MonoBehaviour
{
    //Only have one of these checked
    //Text
    public bool ShowMoney = false;
    public bool ShowRapidBuy = false;
    public bool ShowMissileBuy = false;
    public bool ShowRailBuy = false;
    public bool ShowLaserBuy = false;

    //Mesh Renderer
    public bool ShowRapidUnlockView = false;
    public bool ShowMissileUnlockView = false;
    public bool ShowRailUnlockView = false;
    public bool ShowLaserUnlockView = false;
    public bool ShowRegularUnitView = false;
    public bool ShowRapidUnitView = false;
    public bool ShowMissileUnitView = false;
    public bool ShowRailUnitView = false;
    public bool ShowLaserUnitView = false;

    //Stat Bar
    public bool ShowShieldDamageUnlockView = false;
    public bool ShowHullDamageUnlockView = false;
    public bool ShowSpeedUnlockView = false;
    public bool ShowFireRateUnlockView = false;

    public bool ShowShieldDamageUnitView = false;
    public bool ShowHullDamageUnitView = false;
    public bool ShowSpeedUnitView = false;
    public bool ShowFireRateUnitView = false;

    public Color NewColor;
    private Color PreviousColor;

    private Text TheText;
    private Image TheImage;
    private MeshRenderer TheMeshRenderer;
    private StatBar TheStatBar;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        TheText = GetComponent<Text>();
        TheImage = GetComponent<Image>();
        TheMeshRenderer = GetComponent<MeshRenderer>();
        TheStatBar = GetComponent<StatBar>();
        if (TheText != null) PreviousColor = TheText.color;
    }

    void Update()
    {
        if(!Tracker.FullyFunctional) return;

        //Text
        if (ShowMoney) TheText.text = Tracker.TheResourceManager.Money[Tracker.ID].ToString();
        if (ShowRapidBuy)
        {
            if (!Tracker.TheResourceManager.UnlockedRapid[Tracker.ID])
            {
                if (TheText.color != PreviousColor) TheText.color = PreviousColor;
                TheText.text = "Buy";
            }
            else
            {
                if (TheText.color != NewColor) TheText.color = NewColor;
                TheText.text = "Unlocked";
            }
        }
        if (ShowMissileBuy)
        {
            if (!Tracker.TheResourceManager.UnlockedMissile[Tracker.ID])
            {
                if (TheText.color != PreviousColor) TheText.color = PreviousColor;
                TheText.text = "Buy";
            }
            else
            {
                if (TheText.color != NewColor) TheText.color = NewColor;
                TheText.text = "Unlocked";
            }
        }
        if (ShowRailBuy)
        {
            if (!Tracker.TheResourceManager.UnlockedRail[Tracker.ID])
            {
                if (TheText.color != PreviousColor) TheText.color = PreviousColor;
                TheText.text = "Buy";
            }
            else
            {
                if (TheText.color != NewColor) TheText.color = NewColor;
                TheText.text = "Unlocked";
            }
        }
        if (ShowLaserBuy)
        {
            if (!Tracker.TheResourceManager.UnlockedLaser[Tracker.ID])
            {
                if (TheText.color != PreviousColor) TheText.color = PreviousColor;
                TheText.text = "Buy";
            }
            else
            {
                if (TheText.color != NewColor) TheText.color = NewColor;
                TheText.text = "Unlocked";
            }
        }

        //Mesh Renderer
        if(ShowRapidUnlockView)
        {
            if (Tracker.CurrentTechView == 1) TheMeshRenderer.enabled = true;
            else TheMeshRenderer.enabled = false;
        }
        if (ShowMissileUnlockView)
        {
            if (Tracker.CurrentTechView == 2) TheMeshRenderer.enabled = true;
            else TheMeshRenderer.enabled = false;
        }
        if (ShowRailUnlockView)
        {
            if (Tracker.CurrentTechView == 3) TheMeshRenderer.enabled = true;
            else TheMeshRenderer.enabled = false;
        }
        if (ShowLaserUnlockView)
        {
            if (Tracker.CurrentTechView == 4) TheMeshRenderer.enabled = true;
            else TheMeshRenderer.enabled = false;
        }
        if(ShowRegularUnitView)
        {
            if(Tracker.BuildingSelected)
            {
                if (Tracker.CurrentUnitView == 0) TheMeshRenderer.enabled = true;
                else TheMeshRenderer.enabled = false;
            }
        }
        if (ShowRapidUnitView)
        {
            if (Tracker.BuildingSelected)
            {
                if (Tracker.CurrentUnitView == 1) TheMeshRenderer.enabled = true;
                else TheMeshRenderer.enabled = false;
            }
        }
        if (ShowMissileUnitView)
        {
            if (Tracker.BuildingSelected)
            {
                if (Tracker.CurrentUnitView == 2) TheMeshRenderer.enabled = true;
                else TheMeshRenderer.enabled = false;
            }
        }
        if (ShowRailUnitView)
        {
            if (Tracker.BuildingSelected)
            {
                if (Tracker.CurrentUnitView == 3) TheMeshRenderer.enabled = true;
                else TheMeshRenderer.enabled = false;
            }
        }
        if (ShowLaserUnitView)
        {
            if (Tracker.BuildingSelected)
            {
                if (Tracker.CurrentUnitView == 4) TheMeshRenderer.enabled = true;
                else TheMeshRenderer.enabled = false;
            }
        }

        //Stat Bar
        if (ShowShieldDamageUnlockView)
        {
            if (Tracker.CurrentTechView == 1) TheStatBar.ValueNameCurrent = "RapidShieldDamage";
            else if (Tracker.CurrentTechView == 2) TheStatBar.ValueNameCurrent = "MissileShieldDamage";
            else if (Tracker.CurrentTechView == 3) TheStatBar.ValueNameCurrent = "RailShieldDamage";
            else if (Tracker.CurrentTechView == 4) TheStatBar.ValueNameCurrent = "LaserShieldDamage";
        }
        if (ShowHullDamageUnlockView)
        {
            if (Tracker.CurrentTechView == 1) TheStatBar.ValueNameCurrent = "RapidHullDamage";
            else if (Tracker.CurrentTechView == 2) TheStatBar.ValueNameCurrent = "MissileHullDamage";
            else if (Tracker.CurrentTechView == 3) TheStatBar.ValueNameCurrent = "RailHullDamage";
            else if (Tracker.CurrentTechView == 4) TheStatBar.ValueNameCurrent = "LaserHullDamage";
        }
        if (ShowSpeedUnlockView)
        {
            if (Tracker.CurrentTechView == 1) TheStatBar.ValueNameCurrent = "RapidSpeed";
            else if (Tracker.CurrentTechView == 2) TheStatBar.ValueNameCurrent = "MissileSpeed";
            else if (Tracker.CurrentTechView == 3) TheStatBar.ValueNameCurrent = "RailSpeed";
            else if (Tracker.CurrentTechView == 4) TheStatBar.ValueNameCurrent = "LaserSpeed";
        }
        if (ShowFireRateUnlockView)
        {
            if (Tracker.CurrentTechView == 1) TheStatBar.ValueNameCurrent = "RapidFireRate";
            else if (Tracker.CurrentTechView == 2) TheStatBar.ValueNameCurrent = "MissileFireRate";
            else if (Tracker.CurrentTechView == 3) TheStatBar.ValueNameCurrent = "RailFireRate";
            else if (Tracker.CurrentTechView == 4) TheStatBar.ValueNameCurrent = "LaserFireRate";
        }
        if (ShowShieldDamageUnitView)
        {
            if (Tracker.CurrentUnitView == 0) TheStatBar.ValueNameCurrent = "RegularShieldDamage";
            else if (Tracker.CurrentUnitView == 1) TheStatBar.ValueNameCurrent = "RapidShieldDamage";
            else if (Tracker.CurrentUnitView == 2) TheStatBar.ValueNameCurrent = "MissileShieldDamage";
            else if (Tracker.CurrentUnitView == 3) TheStatBar.ValueNameCurrent = "RailShieldDamage";
            else if (Tracker.CurrentUnitView == 4) TheStatBar.ValueNameCurrent = "LaserShieldDamage";
        }
        if (ShowHullDamageUnitView)
        {
            if (Tracker.CurrentUnitView == 0) TheStatBar.ValueNameCurrent = "RegularHullDamage";
            else if (Tracker.CurrentUnitView == 1) TheStatBar.ValueNameCurrent = "RapidHullDamage";
            else if (Tracker.CurrentUnitView == 2) TheStatBar.ValueNameCurrent = "MissileHullDamage";
            else if (Tracker.CurrentUnitView == 3) TheStatBar.ValueNameCurrent = "RailHullDamage";
            else if (Tracker.CurrentUnitView == 4) TheStatBar.ValueNameCurrent = "LaserHullDamage";
        }
        if (ShowSpeedUnitView)
        {
            if (Tracker.CurrentUnitView == 0) TheStatBar.ValueNameCurrent = "RegularSpeed";
            else if (Tracker.CurrentUnitView == 1) TheStatBar.ValueNameCurrent = "RapidSpeed";
            else if (Tracker.CurrentUnitView == 2) TheStatBar.ValueNameCurrent = "MissileSpeed";
            else if (Tracker.CurrentUnitView == 3) TheStatBar.ValueNameCurrent = "RailSpeed";
            else if (Tracker.CurrentUnitView == 4) TheStatBar.ValueNameCurrent = "LaserSpeed";
        }
        if (ShowFireRateUnitView)
        {
            if (Tracker.CurrentUnitView == 0) TheStatBar.ValueNameCurrent = "RegularFireRate";
            else if (Tracker.CurrentUnitView == 1) TheStatBar.ValueNameCurrent = "RapidFireRate";
            else if (Tracker.CurrentUnitView == 2) TheStatBar.ValueNameCurrent = "MissileFireRate";
            else if (Tracker.CurrentUnitView == 3) TheStatBar.ValueNameCurrent = "RailFireRate";
            else if (Tracker.CurrentUnitView == 4) TheStatBar.ValueNameCurrent = "LaserFireRate";
        }
    }
}
