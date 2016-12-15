using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VisibleSwitch : MonoBehaviour
{
    public string ValueToWatch = "";

    private Text TheText;
    private Image TheImage;
    private Button TheButton;
    private MeshRenderer TheMeshRenderer;

    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();

        TheText = GetComponent<Text>();
        TheImage = GetComponent<Image>();
        TheButton = GetComponent<Button>();
        TheMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (!Tracker.FullyFunctional) return;
        bool visible = (bool)Tracker.GetType().GetField(ValueToWatch).GetValue(Tracker);

        if(visible)
        {
            if (TheText != null) TheText.enabled = true;
            else if (TheImage != null)
            {
                if (TheButton != null)
                {
                    TheImage.enabled = true;
                    TheButton.enabled = true;
                }
                else TheImage.enabled = true;
            }
            else if (TheMeshRenderer != null) TheMeshRenderer.enabled = true;
        }
        else
        {
            if (TheText != null) TheText.enabled = false;
            else if (TheImage != null)
            {
                if (TheButton != null)
                {
                    TheImage.enabled = false;
                    TheButton.enabled = false;
                }
                else TheImage.enabled = false;
            }
            else if (TheMeshRenderer != null) TheMeshRenderer.enabled = false;
        }
    }
}
