using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ClawTimer : MonoBehaviour
{
    public GameObject clawTimerDisplay;
    SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[10];
    public Sprite empty;

    public ClawManager manager;

    public float clawTimerDuration;
    public float clawTimerCount;
    private int clawTimerInt;
    public bool clawTimerRunning = false;
    
    //public TMP_Text clawTimerUI;

    public static event Action ClawTimerEnded;


    private void Awake()
    {
        spriteRenderer = clawTimerDisplay.GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        clawTimerCount = clawTimerDuration;
        // sub to StartClawTimer event
        ClawManager.StartClawTimer += clawTimerStart;

        ClawManager.StopClawTimer += ResetTimer;

    }
    public void clawTimerStart()
    {
        clawTimerCount = clawTimerDuration;
        clawTimerRunning = true;
        IntToSprite();
    }

    /*
    private void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(clawTimer);
        clawTimerUI.text = seconds.ToString();
    }
    */
    private void Update()
    {
        if (clawTimerRunning)
        {
            Debug.Log($"claw timer deltatime {Time.deltaTime}");
            clawTimerCount -= Time.deltaTime;
            IntToSprite();
            Debug.Log("clawTimer = " + clawTimerCount);

            if (clawTimerCount <= 0)
            {
                clawTimerCount = 0;
                clawTimerRunning = false;
                Debug.Log("Claw Timer Finished");
                ClawTimerEnded?.Invoke();
                
                IntToSprite();
            }
        }

        
        if (manager.resettingClawTimer == true)
        {
            manager.resettingClawTimer = false;
            clawTimerCount = clawTimerDuration;
            IntToSprite();
        }
        
    }

    public void ResetTimer()
    {
        clawTimerRunning = false;

        IntToSprite();
    }

    private void IntToSprite()
    {
        clawTimerInt = Mathf.CeilToInt(clawTimerCount);

        Sprite newSprite = sprites[clawTimerInt];
        spriteRenderer.sprite = newSprite;
    }


    
}
