using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float barResetDelay = .5f;
    public float moveBackDelay = .5f;
    public float moveSpeed = 1;
    public float dropOffSpeed = 7;

    public GameObject Bar;
    public GameObject Cable;

    public GameObject dropOff;
    public GameObject center;

    private bool isResetting= false;
    private bool droppedOff = false;

    private Vector3 topMove = Vector3.zero;
    private Vector3 bottomMove = Vector3.zero;

    public Magnet magnet;
    public RoundTimer roundTimer;
    public bool controllable;
    public bool isMagnetized = false;
    
    private void Awake()
    {
        controllable = false;
    }
    // Update is called once per frame
    void Update()
    {
        controllable = roundTimer.controllable;
        isMagnetized = magnet.magnetizing;
        if (isMagnetized)
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
           isResetting = true;   
        }
        

        if (isResetting)
        {
            //StartCoroutine(BarDelay(barResetDelay));
            MoveTo(dropOff);
        }
        if (droppedOff)
        {
            //StartCoroutine(MoveBackDelay(moveBackDelay));
            MoveTo(center);
        }

       
    }


   




    private void MoveTo(GameObject position)
    {
        controllable = false;
        moveSpeed = 1f;
        
        Vector3 moveToZ = new Vector3(Bar.transform.position.x, Bar.transform.position.y, position.transform.position.z);
        Vector3 moveToX = new Vector3(position.transform.position.x, Cable.transform.position.y, Cable.transform.position.z);

        if (Vector3.Distance(Bar.transform.position, moveToZ) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;

            Bar.transform.position = Vector3.MoveTowards(Bar.transform.position, moveToZ, step);
        }
        
        
        else if (Vector3.Distance(Cable.transform.position, moveToX) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;

            Cable.transform.position = Vector3.MoveTowards(Cable.transform.position, moveToX, step);
        }
       
        else 
        {
            Debug.Log("reset");
            isResetting = false;
            droppedOff = true;
        }
        
    }

    IEnumerator BarDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
    }

    IEnumerator MoveBackDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
    }

}
