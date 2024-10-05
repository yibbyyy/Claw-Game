using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GenericBomb : MonoBehaviour, IDisposable
{
    public GameObject InnerExplosion, OuterExplosion, BombImage;

    public TMP_Text bombTimerUI;


    public bool exploded = false;
    public float bombTimerDuration = 5;

    public float smallExplosionDelay = .15f;
    public float largeExplosionDelay = .2f;
    //public static event Action BombExploded;
   public IEnumerator BombExplosion()
    {
        // Set exploded var to true in order for others to not call this function
        exploded = true;
        // TODO
        // start playing sound and maybe send an event that this happened

        // Show inner explosion
        InnerExplosion.gameObject.SetActive(true);

        // Delay
        float elapsedTime = 0;

        while (elapsedTime < smallExplosionDelay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Turn off bomb image
        BombImage.gameObject.SetActive(false);
        // Show outer explosion
        InnerExplosion.SetActive(false);
        OuterExplosion.SetActive(true);

        elapsedTime = 0;
        while (elapsedTime < smallExplosionDelay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OuterExplosion.SetActive(false);
        // Deactivate this game object
        // Inheriting class is responsible for performing cleanup
        Dispose();
    }

    /* Cases include: Finishing before timer end
     * Losing before timer ends
     * Timer ending and losing
     */
     
    /* When winning before timer ends, we have to render the timer useless by unsubbing from event.
     * Same when losing before timer ends, unsubb and call bomb explosion directly.
     * If timer goes off then we need to make sure not to call bomb explosion directly, which hopefully will happen after the explosion executes
     * Just in case, I can make a exploded class boolean that is false until called by explosion coroutine which the state machine and timer can check to ensure that they aren't both firing.
     */
    

    public IEnumerator Timer(float duration)
    {
        float timeremaining = duration;
        
        while (timeremaining > 0)
        {
            bombTimerUI.text = timeremaining.ToString("F2");
            timeremaining -= Time.deltaTime;
            yield return null;
        }
        bombTimerUI.text = "0.00";
        if (!exploded)
        {
            // Play event that bomb blew up
            StartCoroutine(BombExplosion());
            Debug.Log("ran out of time");
        }
        



    }
    public virtual void Dispose()
    {
        BombImage.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}