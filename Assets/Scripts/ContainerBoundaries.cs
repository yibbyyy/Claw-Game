using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBoundaries : MonoBehaviour
{
    public GameObject Bar;
    private Rigidbody BarBody;
    public GameObject Cable;
    private Rigidbody CableBody;



    void Start()
    {
        BarBody = Bar.GetComponent<Rigidbody>();
        CableBody = Cable.GetComponent<Rigidbody>();
    }




    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Bar")
        {
            BarBody.velocity = Vector3.zero;
            CableBody.velocity = Vector3.zero;
        }
        if(collision.gameObject.name == "Cable")
        {
            CableBody.velocity = Vector3.zero;
        }
    }
}
