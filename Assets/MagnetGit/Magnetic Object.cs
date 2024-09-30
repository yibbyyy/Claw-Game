using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    public enum Pole
    {
        North,
        South
    }

    public float MagnetForce;
    public Pole MagneticPole;
    public Rigidbody RigidBody;

    // Use this for initialization
    void Start ()
    {
        RigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnDrawGizmos()
    {

    }
}
