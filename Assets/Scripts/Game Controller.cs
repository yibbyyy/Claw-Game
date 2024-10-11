using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action Setup;
    public static event Action FirstRound;


    public State state = State.idle;

    public enum State
    {
        idle,
        setup,
        inRound,
        endStep,
        roundEnd,
        gameOver
    }

    private void Awake()
    {
        
    }

    void Start()
    {
        Setup?.Invoke();
        GameTimer.timedOut += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameOver()
    {
        Debug.Log("GameOver");
        state = State.gameOver;

    }
}
