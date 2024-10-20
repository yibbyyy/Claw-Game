using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GenericBomb : MonoBehaviour, IDisposable
{
    public GameObject InnerExplosion, OuterExplosion, BombCanvas, humanStickyNote, alienStickyNote;
    public UnityEngine.UI.Image bombImage;

    public Sprite bomb, alienBomb;
    public TMP_Text bombTimerUI;


    public bool exploded = false;
    public float bombTimerDuration = 5;

    public float smallExplosionDelay = .15f;
    public float largeExplosionDelay = .2f;
    public static event Action BombExploded;
    
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
        // Turn off bomb canvas
        BombCanvas.SetActive(false);
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
        BombExploded.Invoke();

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
            bombTimerUI.text = timeremaining.ToString("F0");
            timeremaining -= Time.deltaTime;
            yield return null;
        }
        bombTimerUI.text = "0";
        if (!exploded)
        {
            // Play event that bomb blew up
            StartCoroutine(BombExplosion());
            Debug.Log("ran out of time");
        }
        



    }

    public void SwitchToHBombSprite(Sprite bombSprite)
    {
        bombImage.sprite = bombSprite;
        humanStickyNote.SetActive(true);
        alienStickyNote.SetActive(false);

        
    }

    public void SwitchToABombSprite(Sprite bombSprite)
    {
        bombImage.sprite = bombSprite;
        humanStickyNote.SetActive(false);
        alienStickyNote.SetActive(true);
    }

    public virtual void Dispose()
    {
        BombCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
