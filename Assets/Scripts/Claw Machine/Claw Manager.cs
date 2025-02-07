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

    public ClawTimer clawTimer;
    public bool resettingClawTimer = false;
    public StartButton startButton;

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

    /*
     * Local Actions if we wanted to track these events.
    public event Action StartReset;
    public event Action MovedToDropBox;
    */
    Vector3 barMove = Vector3.zero;
    Vector3 cableMove = Vector3.zero;

    private float minX, maxX, minZ, maxZ;
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
        

        minX = Cable.transform.position.x - 1.5f;
        maxX = Cable.transform.position.x + 1.5f;
        // Back of box
        maxZ = 3.25f;
        // Front of box
        minZ = .25f;
    }

    

    void Update()
    {
        if (currentState == State.idle)
        {
            if (!isSubscribedToButton)
            {
                StartButton.click += OnStartButtonClicked;
                isSubscribedToButton = true;
            }
        }

        else if (currentState == State.waitingForInput)
        {
            ProcessInputs();
        }

        else if (currentState == State.resetting)
        {
            // All coroutines are running in events
            if (magnetizing)
            {
                Debug.Log("Called Magnetize");
                Magnetize();
            }
        }
        else if (magnetizing)
        {
            Debug.Log("Called Magnetize");
            Magnetize();
        }
    }

    
    void ProcessInputs()
    {
        
        // Moving Claw
        barMove.z = Input.GetAxisRaw("Vertical");
        cableMove.x = Input.GetAxisRaw("Horizontal");

        barMove = Vector3.ClampMagnitude(-barMove, .1f);
        cableMove = Vector3.ClampMagnitude(-cableMove, .1f);

        if (Bar.transform.position.z >= maxZ)
        {
            //Debug.Log("Got to MaxZ");
            if (barMove.z > 0)
            {
                //Debug.Log("Got through both if statements z: " + barMove.z);
                Bar.transform.Translate(-barMove * moveSpeed * Time.deltaTime);
            }
        }

        if (Bar.transform.position.z <= minZ)
        {
            if (barMove.z < 0)
            {
                Bar.transform.Translate(-barMove * moveSpeed * Time.deltaTime);
            }
        }
        if (Bar.transform.position.z < maxZ && Bar.transform.position.z > minZ)
        {
            Bar.transform.Translate(-barMove * moveSpeed * Time.deltaTime);
        }
        //Bar.transform.Translate(-barMove * moveSpeed * Time.deltaTime);


        //Debug.Log(Cable.transform.position.x);
        //Debug.Log("Max x is: " + maxX);
        
        if (Cable.transform.position.x >= maxX)
            {
            //Debug.Log("Got to MaxX");
                if (cableMove.x > 0)
                {
                //Debug.Log("Got through both if statements maxx: " + cableMove.x);
                    Cable.transform.Translate(cableMove * moveSpeed * Time.deltaTime);
                }
            }

        if (Cable.transform.position.x <= minX)
            {
                if (cableMove.x < 0)
                {
                //Debug.Log("Got through both if statements minx: " + cableMove.x);
                Cable.transform.Translate(cableMove * moveSpeed * Time.deltaTime);
                }
            }
        if (Cable.transform.position.x < maxX && Cable.transform.position.x > minX && cableMove != Vector3.zero)
        {
            //Debug.Log("Doing 3rd: " + cableMove.x);
            
            Cable.transform.Translate(cableMove * moveSpeed * Time.deltaTime);
        }
        
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
        Debug.Log($"StartButton Clicked!");
        // invoke event to start timer
        StartClawTimer?.Invoke();

        // Unsub from the button
        StartButton.click -= OnStartButtonClicked;
        isSubscribedToButton = false;
        // Set State to Wait for Input
        StartCoroutine(ExitStateDelay());
    }

    public void ClawTimerEnded()
    {
        //SyncMagnetPos();
        // Set state to Reset 
        currentState = State.resetting;
        StartMagnet();
    }

    public Transform magnetMid, magnetLeft, magnetRight;
    
   
    public float radius, maxDistance;
    public LayerMask ignoredLayers;
    public RaycastHit[] hits = new RaycastHit[15];
    public bool magnetizing = false;
    public float attractionStrength;
    public void Magnetize()
    {
        Rigidbody hitBody;
        int numHits = Physics.CapsuleCastNonAlloc(magnetLeft.position, magnetRight.position, radius, Vector3.down, hits, maxDistance, ~ignoredLayers);

        for(int i = 0; i < numHits; i++)
        {
            /*
            if (hits[i].collider == null)
                continue;
            */
            Debug.Log("Hit a " + hits[i].collider.name);
           
            Vector3 forcedDirection = magnetMid.position - hits[i].collider.transform.position;
            if (hits[i].collider.TryGetComponent<Rigidbody>(out hitBody))
            {
                hitBody.AddForce(forcedDirection.normalized * attractionStrength);
            }
            
            
        }
        
    }
    private void OnDrawGizmos()
    {
        // Set Gizmo color
        Gizmos.color = Color.green;

        if (magnetizing)
        {
            // Draw the starting and ending spheres
            Gizmos.DrawWireSphere(magnetLeft.position, radius);
            Gizmos.DrawWireSphere(magnetRight.position, radius);

            // Draw the capsule's body (cylinder-like connection between spheres)
            DrawCapsuleBetweenPoints(magnetLeft.position, magnetRight.position, radius);

            // Draw the downward movement (cast direction is -Y)
            Gizmos.color = Color.red;
            Vector3 castEndStart = magnetLeft.position + Vector3.down * maxDistance;
            Vector3 castEndEnd = magnetRight.position + Vector3.down * maxDistance;

            Gizmos.DrawWireSphere(castEndStart, radius);
            Gizmos.DrawWireSphere(castEndEnd, radius);
            DrawCapsuleBetweenPoints(castEndStart, castEndEnd, radius);

            // Draw a line representing the downward movement
            Gizmos.DrawLine(magnetLeft.position, castEndStart);
            Gizmos.DrawLine(magnetRight.position, castEndEnd);
        }
        
    }

    private void DrawCapsuleBetweenPoints(Vector3 p1, Vector3 p2, float radius)
    {
        Vector3 dir = (p2 - p1).normalized;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS((p1 + p2) / 2, rot, new Vector3(radius * 2, (p2 - p1).magnitude / 2, radius * 2));

        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 1));
        Gizmos.matrix = oldMatrix;
    }
    private void StartMagnet()
    {
        SyncMagnetPos();
        // Turn Magnet On
        
       
        ChangeMagnetColor(Color.red);
        
        magnetizing = true;
        
        //World.Permeability = startingPermiability;

        startMagnetizing?.Invoke();
    }
    private void StopMagnet()
    {
        magnetizing = false;
        ChangeMagnetColor(Color.white);
        World.Permeability = 0;
        // Can invoke a stop magnet event if needed
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
        float duration = 0;
        while (duration < pickupTime)
        {
            duration += Time.deltaTime;
            yield return null;
        }
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
        duration = 0;
        while (duration < dropOffDelay)
        {
            duration += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Start drop");
        StopMagnet();
        StartCoroutine(DropOffToCenter());

    }

    IEnumerator DropOffToCenter()
    {
        resettingClawTimer = true;
        
        // Wait for items to fall
        float duration = 0;
        while (duration < dropOffTime)
        {
            duration += Time.deltaTime;
            yield return null;
        }

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
        startButton.clickable = true;
    }

    IEnumerator ExitStateDelay()
    {
        float duration = 0;
        while (duration < .1)
        {
            duration += Time.deltaTime;
            yield return null;
        }

        currentState = State.waitingForInput;
    }
}
