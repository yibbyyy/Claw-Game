using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic1 : MonoBehaviour
{
    public float magneticStrength = 1.0f;
    public float maxMagneticStrngth = 10f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //rb.useGravity = true;
    }
}
