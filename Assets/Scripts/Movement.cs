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


    private void Awake()
    {
        BarBody = Bar.GetComponent<Rigidbody>();
        CableBody = Cable.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        

        topMove.x = Input.GetAxisRaw("Vertical");
        bottomMove.z = Input.GetAxisRaw("Horizontal");

        topMove = Vector3.ClampMagnitude(topMove, .1f);
        bottomMove = Vector3.ClampMagnitude(bottomMove, .1f);

        Bar.transform.Translate(topMove * moveSpeed * Time.deltaTime);
        Cable.transform.Translate(bottomMove * moveSpeed * Time.deltaTime);
    }


}
