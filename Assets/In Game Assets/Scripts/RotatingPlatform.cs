using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{    
    public bool clockwise;

    public Vector3 eulerAngleVelocity;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        int _turnWay = clockwise ? 1 : -1;
        eulerAngleVelocity *= _turnWay;
    }

    private void FixedUpdate()
    {        
        rb.angularVelocity = eulerAngleVelocity;
    }
}
