using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class ClawAudio : MonoBehaviour
{
    Vector3 currentPos;
    Vector3 lastPos;

    bool stopPrimed = false;

    public AudioClip run;
    public AudioClip stop;

    AudioSource source;

     
    void Awake()
    {
        source = GetComponent<AudioSource>();

        

        currentPos = transform.position;
        lastPos = transform.position;
    }
    void Update()
    {


        currentPos = transform.position;
        if (currentPos == lastPos && source.isPlaying && stopPrimed)
        {
            stopPrimed = false;

            StartCoroutine(StopClip());
            Debug.Log("no audio");

        }

        else if (currentPos != lastPos && !source.isPlaying)
        {
            stopPrimed = true;
            StartCoroutine(StartClip());
            Debug.Log("yes audio");
        }
        lastPos = transform.position;

    }

    IEnumerator StartClip()
    {
        source.loop = true;
        source.PlayOneShot(run);
        yield return null;

    }

    IEnumerator StopClip()
    {
        Debug.Log("stopClip called");
        source.Stop();
        source.loop = false;
        source.PlayOneShot(stop); 
        yield return null;
    }
}
