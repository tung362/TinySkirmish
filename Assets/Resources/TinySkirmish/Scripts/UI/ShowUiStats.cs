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

    public Color NewColor;
    private Color PreviousColor;

    private Text TheText;
    private MeshRenderer TheMeshRenderer;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        TheText = GetComponent<Text>();
        TheMeshRenderer = GetComponent<MeshRenderer>();
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
    }
}
