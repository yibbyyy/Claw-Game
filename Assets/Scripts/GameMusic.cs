using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMusic : MonoBehaviour
{
    public AudioClip track1, track2;
    AudioSource source;
    float volume, startVolume;
    public float deltaVolume;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
        source.clip = track1;
        source.Play();

        StartButton.click += StartPlayback;
    }
    
    void StartPlayback()
    {
        volume = source.volume;
        startVolume = volume;

        StartCoroutine(PlayOut(volume));
        

        StartButton.click -= StartPlayback;
    }


    //smooth transition to main track
    private IEnumerator PlayOut(float volume)
    {

        Debug.Log($"GameMusic PlayOut(volume {volume}) called");
        while (volume > 0) 
        {
            volume -= deltaVolume;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (volume <= 0) 
        {
            source.clip = track2;
            StartCoroutine(PlayIn(volume));
            yield return null;
        }
        
    }

    private IEnumerator PlayIn(float volume)
    {
        Debug.Log($"GameMusic PlayIn(volume {volume}) called");
        while (volume < startVolume) 
        {
            volume += deltaVolume;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (volume >= startVolume)
        {
            
            volume = startVolume;
            yield return null;
        }
        
    }


}
