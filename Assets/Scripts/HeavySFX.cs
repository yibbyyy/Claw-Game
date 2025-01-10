using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySFX : MonoBehaviour
{
    AudioSource source; 
    // Start is called before the first frame update

    AudioClip clip;


    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!source.isPlaying)
        {
            float speed = Mathf.Sqrt(Mathf.Pow(collision.relativeVelocity.x, 2f) + Mathf.Pow(collision.relativeVelocity.y, 2f) + Mathf.Pow(collision.relativeVelocity.z, 2f)) / 2f;
            //Debug.Log("speed = " + speed);
            if (speed > 0.05)
            {
                source.volume = source.volume * speed;
                source.Play();
            }

        }

    }
}
