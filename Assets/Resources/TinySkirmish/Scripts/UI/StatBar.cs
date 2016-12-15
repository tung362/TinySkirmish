using UnityEngine;
using System.Collections;

public class StatBar : MonoBehaviour
{
    //(String to variable convertion) type out the name of the varible you want to use
    public string ValueNameCurrent = "CurrentHealth";
    public string ValueNameMax = "MaxHealth";
    public bool Horizontal = true;
    //If stat bar should transition instantly or slowly
    public bool UseStaticBarChange = false;
    public bool ReverseStaticOnIncrease = false;
    private Vector3 StartingScale;

    private float FakeProgress = 1;
    private float FakeVelo = 0;
    public float Smoothness = 0.3f;

    //Used if ReverseStaticOnIncrease is true, tracks if stat values is increasing or decreasing
    private float CurrentChanges = 1;
    private bool RunOnce = true;

    //Access to global vars
    private ManagerTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<ManagerTracker>();
        StartingScale = transform.localScale;
        CurrentChanges = (float)Tracker.GetType().GetField(ValueNameCurrent).GetValue(Tracker);
    }

    void FixedUpdate()
    {
        if (!Tracker.FullyFunctional) return;

        float current = (float)Tracker.GetType().GetField(ValueNameCurrent).GetValue(Tracker);
        float max = (float)Tracker.GetType().GetField(ValueNameMax).GetValue(Tracker);

        //Reverse static
        if (ReverseStaticOnIncrease == true)
        {
            //print(FakeVelo);
            if (CurrentChanges < current && RunOnce == true)
            {
                if (UseStaticBarChange == true) UseStaticBarChange = false;
                else UseStaticBarChange = true;
                RunOnce = false;
            }
            else
            {
                if (RunOnce == false)
                {
                    if (UseStaticBarChange == true)
                    {
                        if (UseStaticBarChange == true) UseStaticBarChange = false;
                        else UseStaticBarChange = true;
                        RunOnce = true;
                    }
                    else if (ReverseStaticOnIncrease == true)
                    {
                        //print(FakeProgress + "/" + (Realprogress - Mathf.Epsilon));
                        if (FakeProgress >= (current / max) - Mathf.Epsilon)
                        {
                            if (RunOnce == false)
                            {
                                if (UseStaticBarChange == true) UseStaticBarChange = false;
                                else UseStaticBarChange = true;
                                RunOnce = true;
                            }
                        }
                    }
                }
            }
        }

        if (UseStaticBarChange == true)
        {
            //Get a percentage of bar value left
            float progress = (current / max);
            FakeProgress = progress;
            Vector3 modifiedScale = StartingScale;
            if (Horizontal == true) modifiedScale = new Vector3(StartingScale.x * FakeProgress, StartingScale.y, StartingScale.z);
            else modifiedScale = new Vector3(StartingScale.x, StartingScale.y * FakeProgress, StartingScale.z);
            transform.localScale = modifiedScale;
        }
        else
        {
            float progress = (current / max);
            //Get a percentage of bar value left
            FakeProgress = Mathf.SmoothDamp(FakeProgress, progress, ref FakeVelo, Smoothness);
            Vector3 modifiedScale = StartingScale;
            if (Horizontal == true) modifiedScale = new Vector3(StartingScale.x * FakeProgress, StartingScale.y, StartingScale.z);
            else modifiedScale = new Vector3(StartingScale.x, StartingScale.y * FakeProgress, StartingScale.z);
            transform.localScale = modifiedScale;
        }
        CurrentChanges = current;
    }
}
