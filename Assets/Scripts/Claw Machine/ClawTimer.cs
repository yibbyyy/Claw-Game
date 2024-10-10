using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ClawTimer : MonoBehaviour
{
    public GameObject clawTimerDisplay;
    Transform digit;

    public float clawTimerDuration;
    public float clawTimer;
    private int clawTimerInt;
    public bool clawTimerRunning = false;
    
    public TMP_Text clawTimerUI;

    public static event Action ClawTimerEnded;


    private void Awake()
    {
        digit = clawTimerDisplay.GetComponentInChildren<Transform>();
        Debug.Log("transform = " + digit.name);
    }
    private void Start()
    {
        clawTimer = clawTimerDuration;
        // sub to StartClawTimer event
        ClawManager.StartClawTimer += clawTimerStart;

        ClawManager.StopClawTimer += ResetTimer;

    }
    public void clawTimerStart()
    {
        clawTimerRunning = true;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(clawTimer);
        clawTimerUI.text = seconds.ToString();
    }

    private void Update()
    {
        if (clawTimerRunning)
        {
            clawTimer -= Time.deltaTime;
            UpdateTimerUI();

            if (clawTimer <= 0)
            {
                clawTimer = 0;
                clawTimerRunning = false;
                Debug.Log("Claw Timer Finished");
                ClawTimerEnded?.Invoke();

                clawTimer = clawTimerDuration;
                UpdateTimerUI();
            }
        }
    }

    public void ResetTimer()
    {
        clawTimerRunning = false;

        clawTimer = clawTimerDuration;
        UpdateTimerUI();
    }

    private void IntToSprite()
    {
        clawTimerInt = Mathf.RoundToInt(clawTimer);


        /*
        for (int i = 0; i < places.Count; i++)
        {
            //If the string is shorter than the expected length, adds a 0 to the left
            string tempString = timerCountString.PadLeft(places.Count, '0');
            //Gets sprite renderer from the indexed child 
            SpriteRenderer spriteRenderer = transform.GetChild(i).GetComponent<SpriteRenderer>();

            //Takes passed string and indexes into it, returning the integer at that index
            //Indexing into a string returns the character as unicode(ASCII)
            //By subtracting the ASCII number for 0(48) from any int 0-9, the integer will be returned as an int
            int dig = tempString[i] - '0';
            Sprite newSprite = sprites[dig];
            spriteRenderer.sprite = newSprite;
        }
        */
    }
}
