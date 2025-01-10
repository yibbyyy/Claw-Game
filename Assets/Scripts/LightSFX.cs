using System;
using UnityEngine;

public class LightSFX : MonoBehaviour
{
    AudioSource source;
    public float defaultVolume;


    void OnEnable()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        string tag = collision.gameObject.tag;
        float speed = collision.relativeVelocity.magnitude; 
        if (!source.isPlaying && speed > 3f)
        {

            if(tag == "ABomb" || tag == "Bomb" || tag == "Chest" || tag == "Untagged")
            {
                source.volume = source.volume / 4f;
            } 
                
            //Debug.Log($"speed is {speed}");
            float adjustedSpeed = Mathf.Clamp(Mathf.Log(speed - 1.5f, 2f) / 3f, 0.01f, 1f);
            

            source.volume = source.volume * adjustedSpeed;
            source.pitch = UnityEngine.Random.Range(0f, 2f);

            //Debug.Log($"tag = {tag} and speed is {adjustedSpeed} & volume is {source.volume}");
            //Debug.Log($"pitch is {source.pitch}");
            source.Play();

            //reset volue and pitch
            source.volume = defaultVolume;
            source.pitch = 1f;
        }

    }
}
