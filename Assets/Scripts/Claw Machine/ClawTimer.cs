using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ClawTimer : MonoBehaviour
{
    
    public float clawTimerDuration;
    public float clawTimer;
    public bool clawTimerRunning = false;
    
    public TMP_Text clawTimerUI;

    public static event Action ClawTimerEnded;
    
    private void Start()
    {
        clawTimer = clawTimerDuration;
        // sub to StartClawTimer event
        ClawManager.StartClawTimer += clawTimerStart;

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

    
    
}
