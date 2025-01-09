using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMusic : MonoBehaviour
{
    public AudioClip track;
    public AudioClip aTrack;
    AudioSource source;


    void Start()
    {
        StartButton.click += StartPlayback;
        source = GetComponent<AudioSource>();
    }

    void StartPlayback()
    {
        source.clip = track;
        source.Play();
        StartButton.click -= StartPlayback;
    }
}
