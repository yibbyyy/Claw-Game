using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    public GameObject dropBox;
    private Collider boxCollider;

    private void Awake()
    {
        boxCollider = dropBox.GetComponent<Collider>();
    }


    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.gameObject.name);
    }
}
