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
    public static event Action Refill;
    public GameObject GameOverUI;

    public State state = State.idle;

    public enum State
    {
        idle,
        setup,
        inRound,
        endStep,
        roundEnd,
        refilling,
        gameOver
    }

    private void Awake()
    {
        GameTimer.timedOut += GameOver;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("OnEnable called");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Setup += SetUpGame;
        Setup.Invoke();
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);


    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void GameOver()
    {
        Debug.Log("GameOver");
        _dropBox.collectingItems = false;
        GameOverUI.SetActive(true);
        //state = State.gameOver;

    }

    public DropBox _dropBox;
    void SetUpGame()
    {
        Debug.Log("Setting up");
        _dropBox.collectingItems = true;
    }
}
