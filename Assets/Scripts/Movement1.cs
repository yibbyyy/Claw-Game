using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Movement1 : MonoBehaviour
{
    public float barResetDelay = .5f;
    public float moveBackDelay = 2f;
    public float magnetTurnOffDelay = 1f;
    public float moveSpeed = 1;
    public float dropOffSpeed = 7;

    public GameObject Bar;
    public GameObject Cable;

    public GameObject dropOff;
    public GameObject center;

    
    private bool droppedOff = false;

    private Vector3 topMove = Vector3.zero;
    private Vector3 bottomMove = Vector3.zero;

    public Magnet1 magnet;
    public RoundTimer roundTimer;
    public bool isResetting = false;
    public bool hasResetStarted = false;
    public bool controllable;
    public bool isMagnetized = false;

    public bool readyToMoveBox = false;
    public bool readyToMoveCenter = false;
    public bool moveToCenterStarted = false;
    public bool moving = false;
    
    private void Awake()
    {
        controllable = false;
    }
    // Update is called once per frame
    void Update()
    {
        controllable = roundTimer.controllable;
        isMagnetized = magnet.magnetizing;
        if (isMagnetized && controllable == true)
        {
            controllable = false;
            isResetting = true;
        }
        // if magnetized stop control and start resetting
        if (controllable)
        {
            
            moveSpeed = 7;
            topMove.z = Input.GetAxisRaw("Vertical");
            bottomMove.x = Input.GetAxisRaw("Horizontal");

            topMove = Vector3.ClampMagnitude(-topMove, .1f);
            bottomMove = Vector3.ClampMagnitude(-bottomMove, .1f);

            Bar.transform.Translate(topMove * moveSpeed * Time.deltaTime);
            Cable.transform.Translate(bottomMove * moveSpeed * Time.deltaTime);
        }
        
        if (Input.GetMouseButtonDown(1) )
        {
            //StartCoroutine(MoveTo(dropOff));
            isResetting = true;

           
        }
        
        
        if (isResetting && !hasResetStarted)
        {
            hasResetStarted = true;
            StartCoroutine(BarDelay(barResetDelay));

            

                
        }
        if (droppedOff && !moveToCenterStarted)
        {
            moveToCenterStarted = true;
            StartCoroutine(MoveBackDelay(moveBackDelay));

            
                
        }
        
       
    }


   




    IEnumerator MoveToDropBox(GameObject position)
    {
        
        controllable = false;
        moving = true;
        moveSpeed = 1f;
        //bool movingBar = true;
        Vector3 moveToZ = new Vector3(Bar.transform.position.x, Bar.transform.position.y, position.transform.position.z);
        

        while (Vector3.Distance(Bar.transform.position, moveToZ) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;

            Bar.transform.position = Vector3.MoveTowards(Bar.transform.position, moveToZ, step);
            yield return null;
        }


        Vector3 moveToX = new Vector3(position.transform.position.x, Cable.transform.position.y, Cable.transform.position.z);
        while (Vector3.Distance(Cable.transform.position, moveToX) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;

            Cable.transform.position = Vector3.MoveTowards(Cable.transform.position, moveToX, step);
            yield return null;
        }
        moving = false;
       
        droppedOff = true;
        //magnet.magnetizing = false;
        StartCoroutine(MagnetTurnOffDelay(magnetTurnOffDelay));
        isResetting = false;
        hasResetStarted = false;
        yield return null;


    }
    IEnumerator MoveToCenter(GameObject position)
    {
        
        controllable = false;
        moveSpeed = 1f;
        moving = true;
        //bool movingBar = true;
        Vector3 moveToZ = new Vector3(Bar.transform.position.x, Bar.transform.position.y, position.transform.position.z);


        while (Vector3.Distance(Bar.transform.position, moveToZ) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;

            Bar.transform.position = Vector3.MoveTowards(Bar.transform.position, moveToZ, step);
            yield return null;
        }


        Vector3 moveToX = new Vector3(position.transform.position.x, Cable.transform.position.y, Cable.transform.position.z);
        while (Vector3.Distance(Cable.transform.position, moveToX) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;

            Cable.transform.position = Vector3.MoveTowards(Cable.transform.position, moveToX, step);
            yield return null;
        }

        droppedOff = false;
        moving = false;
        hasResetStarted = false;
        moveToCenterStarted = false;
        Debug.Log("reset");
        yield return null;

    }
    IEnumerator BarDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(MoveToDropBox(dropOff));
        readyToMoveBox = true;
        yield return null;
    }

    IEnumerator MoveBackDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(MoveToCenter(center));
        readyToMoveCenter = true;
        yield return null;
    }

    IEnumerator MagnetTurnOffDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        magnet.magnetizing = false;
        yield return null;

    }

}
