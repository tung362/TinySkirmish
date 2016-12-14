using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    public Vector3 RotationDirection = Vector3.zero;
    public float Speed = 1;
    public bool Stop = false;

    void FixedUpdate()
    {
        if (Stop == false) transform.Rotate(RotationDirection * Speed * Time.deltaTime);
    }
}
