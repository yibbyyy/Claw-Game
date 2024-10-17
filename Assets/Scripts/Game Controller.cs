using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("OnEnable called");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
    void Start()
    {
        //Setup += SetUpGame;
        //Setup?.Invoke();
        //GameTimer.timedOut += GameOver;
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

    void SetUpGame()
    {
        Debug.Log("Setting up");
    }
}
