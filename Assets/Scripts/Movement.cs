using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveSpeed = 1;

    public GameObject Bar;
    private Rigidbody BarBody;
    public GameObject Cable;
    private Rigidbody CableBody;

    private Vector3 topMove = Vector3.zero;
    private Vector3 bottomMove = Vector3.zero;

    private Vector3 barDestinationX = new Vector3 (-3.49f, 0 , 0);
    private Vector3 magnetDestinationZ = new Vector3 (0, 0, 0.01f);
    
    public RoundTimer roundTimer;
    private bool controllable;

    private void Awake()
    {
        controllable = false;
        BarBody = Bar.GetComponent<Rigidbody>();
        CableBody = Cable.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        controllable = roundTimer.controllable;
        if (controllable)
        {
            moveSpeed = 7;
            topMove.x = Input.GetAxisRaw("Vertical");
            bottomMove.z = Input.GetAxisRaw("Horizontal");

            topMove = Vector3.ClampMagnitude(topMove, .1f);
            bottomMove = Vector3.ClampMagnitude(bottomMove, .1f);

            Bar.transform.Translate(topMove * moveSpeed * Time.deltaTime);
            Cable.transform.Translate(bottomMove * moveSpeed * Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(1))
        {
            
            DropOff(barDestinationX, magnetDestinationZ, Bar.transform.position, Cable.transform.position);
        }
    }


    private void DropOff(Vector3 barDestination, Vector3 magnetDestination, Vector3 barPosition, Vector3 magnetPosition)
    {
        controllable = false;
        moveSpeed = 4;
        
    }

}
