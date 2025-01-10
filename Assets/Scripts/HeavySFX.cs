using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySFX : MonoBehaviour
{
    AudioSource source;
    public float defaultVolume;
    // Start is called before the first frame update

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;
        if (!source.isPlaying && speed > 3f)
        {
            //adjust speed to suit as a volume multiplier
            float adjustedSpeed = Mathf.Clamp(Mathf.Log(speed - 1.5f, 2f) / 3f, 0.01f, 1f);

            //update volume
            source.volume = source.volume * adjustedSpeed;

            //Debug.Log($"speed is {adjustedSpeed} & volume is {source.volume}");
            source.Play();

            //reset volue
            source.volume = defaultVolume;
        }
    }

}
