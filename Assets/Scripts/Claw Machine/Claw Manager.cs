using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClawManager : MonoBehaviour
{
    public GameObject Bar, Cable, Magnet;
    public Button clawMachineStartButton;
    public bool isSubscribedToButton = false;
    public float moveSpeed = 7;

    public static event Action StartClawTimer;
    
    
    Vector3 barMove = Vector3.zero;
    Vector3 cableMove = Vector3.zero;

    public State currentState = State.idle;
    private Vector3 magnetPos;
    public enum State
    {
        idle,
        waitingForInput
    }
    private void Start()
    {
        magnetPos = Magnet.transform.position;

        ClawTimer.ClawTimerEnded += ClawTimerEnded;
    }

    private void ClawTimer_ClawTimerEnded()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (currentState == State.idle)
        {
            if (!isSubscribedToButton)
            {
                clawMachineStartButton.onClick.AddListener(OnStartButtonClicked);
            }
        }
        if (currentState == State.waitingForInput)
        {
            ProcessInputs();
        }
    }

    
    void ProcessInputs()
    {
        
        // Moving Claw
        barMove.z = Input.GetAxisRaw("Vertical");
        cableMove.x = Input.GetAxisRaw("Horizontal");

        barMove = Vector3.ClampMagnitude(-barMove, .1f);
        cableMove = Vector3.ClampMagnitude(-cableMove, .1f);

        
        Bar.transform.Translate(-barMove * moveSpeed * Time.deltaTime);
        Cable.transform.Translate(cableMove * moveSpeed * Time.deltaTime);

        magnetPos.z = Cable.transform.position.z;
        magnetPos.x = Cable.transform.position.x;
        magnetPos.y = Magnet.transform.position.y;
        Magnet.transform.position = magnetPos;

        // Magnetizing

        // Timer Runs out
    }

    public void OnStartButtonClicked()
    {
        // invoke event to start timer
        StartClawTimer?.Invoke();

        // Unsub from the button
        clawMachineStartButton.onClick.RemoveListener(OnStartButtonClicked);

        // Set State to Wait for Input
        currentState = State.waitingForInput;
    }

    public void ClawTimerEnded()
    {
        // Set state to Magnetize just set to idle for now
        currentState = State.idle;
    }
    
}
