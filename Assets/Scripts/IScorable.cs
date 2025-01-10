using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IScorable : MonoBehaviour
{
    public int pointValue;
    public int timeValue;
    public float alienValue;

 
    Rigidbody rb;
    AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


}

