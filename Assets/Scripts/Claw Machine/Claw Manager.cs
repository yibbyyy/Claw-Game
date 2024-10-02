using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class ClawManager : MonoBehaviour
{
    public GameObject Bar, Cable, Magnet, dropoff, center, cableCenter;
    public MagnetWorld World;

    public Button clawMachineStartButton;
    public bool isSubscribedToButton = false;
    public float moveSpeed = 7;
    public float resetSpeed = 1;
    public float startingPermiability = .22f;
    public float movingPermiability = .10f;
    public float pickupTime = 3;
    public float dropOffDelay = 2;
    public float dropOffTime = 3;

    public static event Action StartClawTimer;
    public static event Action startMagnetizing;
    public static event Action StopClawTimer;

    public event Action StartReset;
    public event Action MovedToDropBox;
    Vector3 barMove = Vector3.zero;
    Vector3 cableMove = Vector3.zero;

    public State currentState = State.idle;
    private Vector3 magnetPos;
    

    public enum State
    {
        idle,
        waitingForInput,
        resetting
    }
    private void Start()
    {
        magnetPos = Magnet.transform.position;

        ClawTimer.ClawTimerEnded += ClawTimerEnded;

        startMagnetizing += ResetClaw;
        
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

        else if (currentState == State.waitingForInput)
        {
            ProcessInputs();
        }

        else if (currentState == State.resetting)
        {
            // All coroutines are running in events
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
        SyncMagnetPos();
        

        // Magnetizing
        if (Input.GetMouseButtonDown(0))
        {
            SyncMagnetPos();
            Debug.Log("Called sync mag");
            // This loop only happens if the player clicks before the timer
            // Stop the claw timer
            StopClawTimer?.Invoke();

            ClawTimerEnded();
        }
        
    }

    private void SyncMagnetPos()
    {
       
        Magnet.transform.position = cableCenter.transform.position;
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
        //SyncMagnetPos();
        // Set state to Reset 
        currentState = State.resetting;
        StartMagnet();
    }

    private void StartMagnet()
    {
        //SyncMagnetPos();
        // Turn Magnet On
        ToggleMagnet();
        ChangeMagnetColor(Color.red);
        World.Permeability = startingPermiability;

        startMagnetizing?.Invoke();
    }
    private void StopMagnet()
    {
        ChangeMagnetColor(Color.white);
        World.Permeability = 0;
        // Can invoke a stop magnet event if needed
    }
    private void ToggleMagnet()
    {
        World.enabled = !World.enabled;
    }

    private void ChangeMagnetColor(Color color)
    {
        Magnet.GetComponent<Renderer>().material.color = color;
    }
    
    private void ResetClaw()
    {
        Debug.Log("Called resetClaw");
        StartCoroutine(MoveToBin());
    }

    IEnumerator MoveToBin()
    {
        
        yield return new WaitForSeconds(pickupTime);
        //World.Permeability = movingPermiability;
        Vector3 moveToZ = new Vector3(Bar.transform.position.x, Bar.transform.position.y, dropoff.transform.position.z);
        while (Vector3.Distance(Bar.transform.position, moveToZ) > 0.1f)
        {
            float step = resetSpeed * Time.deltaTime;

            Bar.transform.position = Vector3.MoveTowards(Bar.transform.position, moveToZ, step);
            SyncMagnetPos();
            yield return null;
        }

        Vector3 moveToX = new Vector3(dropoff.transform.position.x, Cable.transform.position.y, Cable.transform.position.z);
        while (Vector3.Distance(Cable.transform.position, moveToX) > 0.1f)
        {
            float step = resetSpeed * Time.deltaTime;

            Cable.transform.position = Vector3.MoveTowards(Cable.transform.position, moveToX, step);
            SyncMagnetPos(); 
            yield return null;
        }
        // Wait a delay for items to settle over box
        yield return new WaitForSeconds(dropOffDelay);
        Debug.Log("Start drop");
        StopMagnet();
        StartCoroutine(DropOffToCenter());

    }

    IEnumerator DropOffToCenter()
    {
        // Wait for items to fall
        yield return new WaitForSeconds(dropOffTime);

        // Start moving towards center
        Vector3 moveToZ = new Vector3(Bar.transform.position.x, Bar.transform.position.y, center.transform.position.z);
        while (Vector3.Distance(Bar.transform.position, moveToZ) > 0.1f)
        {
            float step = resetSpeed * Time.deltaTime;

            Bar.transform.position = Vector3.MoveTowards(Bar.transform.position, moveToZ, step);
            SyncMagnetPos();
            yield return null;
        }

        Vector3 moveToX = new Vector3(center.transform.position.x, Cable.transform.position.y, Cable.transform.position.z);
        while (Vector3.Distance(Cable.transform.position, moveToX) > 0.1f)
        {
            float step = resetSpeed * Time.deltaTime;

            Cable.transform.position = Vector3.MoveTowards(Cable.transform.position, moveToX, step);
            SyncMagnetPos();
            yield return null;
        }

        Debug.Log("fully reset");
        currentState = State.idle;
    }
}
