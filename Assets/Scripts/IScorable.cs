using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IScorable : MonoBehaviour
{
    public int pointValue;
    public GameObject target;
    public float strength;

    public int maxFrameCount;
    private int frameCount = 0;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        frameCount++;
        if (frameCount < maxFrameCount)
            rb.AddForce(Vector3.Normalize(target.transform.position - transform.position) * strength * 2, ForceMode.Force);
    }
}

