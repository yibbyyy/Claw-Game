using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionUI : MonoBehaviour
{
    public GameObject explosionMessageUI;
    public AudioSource _audio;
    public float UILife = 2f;
    public float soundDelay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        GenericBomb.BombExploded += DisplayUIWrapper;
        
    }

    public void DisplayUIWrapper()
    {
        StartCoroutine(DisplayUI());
    }
    IEnumerator DisplayUI()
    {
        // set UI to active
        explosionMessageUI.SetActive(true);
        yield return StartCoroutine(Timer(soundDelay));
        // Play sound
        _audio.Play();
        // Run timer
        yield return StartCoroutine(Timer(UILife));
        // Deactivate UI
        explosionMessageUI.SetActive(false);
    }

    IEnumerator Timer(float delay)
    {
        float duration = 0f;

        while (duration < delay)
        {
            duration += Time.deltaTime;
            yield return null;
        }
    }
    
}
