using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action Setup;
    public static event Action FirstRound;

    public static event Action GameOver;

    
    public enum State
    {
        idle,
        setup,
        inRound,
        endStep,
        roundEnd,
        gameOver
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
