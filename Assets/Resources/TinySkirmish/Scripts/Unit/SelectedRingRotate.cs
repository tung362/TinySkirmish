using UnityEngine;
using System.Collections;

public class SelectedRingRotate : MonoBehaviour
{
    public MeshRenderer Ring1;
    public MeshRenderer Ring2;
    public MeshRenderer Ring3;
    public MeshRenderer Ring4;

    private UnitStats TheUnitStats;
    private Gate TheGate;

    void Start()
    {
        TheUnitStats = transform.root.GetComponent<UnitStats>();
        TheGate = transform.root.GetComponent<Gate>();
    }

    public Vector3 RotationDirection = Vector3.zero;
    public float Speed = 1;

    void FixedUpdate()
    {
        if(TheUnitStats != null)
        {
            if (TheUnitStats.IsSelected)
            {
                if (!Ring1.enabled || !Ring2.enabled || !Ring3.enabled || !Ring4.enabled)
                {
                    Ring1.enabled = true;
                    Ring2.enabled = true;
                    Ring3.enabled = true;
                    Ring4.enabled = true;
                }
                transform.Rotate(RotationDirection * Speed * Time.fixedDeltaTime);
            }
            else
            {
                if (Ring1.enabled || Ring2.enabled || Ring3.enabled || Ring4.enabled)
                {
                    Ring1.enabled = false;
                    Ring2.enabled = false;
                    Ring3.enabled = false;
                    Ring4.enabled = false;
                }
            }
        }

        if (TheGate != null)
        {
            if (TheGate.IsSelected)
            {
                if (!Ring1.enabled || !Ring2.enabled || !Ring3.enabled || !Ring4.enabled)
                {
                    Ring1.enabled = true;
                    Ring2.enabled = true;
                    Ring3.enabled = true;
                    Ring4.enabled = true;
                }
                transform.Rotate(RotationDirection * Speed * Time.fixedDeltaTime);
            }
            else
            {
                if (Ring1.enabled || Ring2.enabled || Ring3.enabled || Ring4.enabled)
                {
                    Ring1.enabled = false;
                    Ring2.enabled = false;
                    Ring3.enabled = false;
                    Ring4.enabled = false;
                }
            }
        }
    }
}
