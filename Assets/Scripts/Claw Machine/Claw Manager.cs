using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClawManager : MonoBehaviour
{
    public GameObject Bar, Cable, Magnet;
    public float moveSpeed = 7;
   

    
    
    Vector3 barMove = Vector3.zero;
    Vector3 cableMove = Vector3.zero;

    private Vector3 magnetPos;
    public enum State
    {
        idle,
        waitingForInput
    }
    private void Start()
    {
        magnetPos = Magnet.transform.position;
    }

    public State currentState = State.waitingForInput;
   
    void Update()
    {
        if (currentState == State.waitingForInput)
        {
            EnterWaitingForInput();
            ProcessInputs();
        }
    }

    void EnterWaitingForInput()
    {
        // Start Timer
        
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

    
}
