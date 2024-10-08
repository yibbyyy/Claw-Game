using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DropBox : MonoBehaviour
{
    public GameObject dropBox;
    private Collider boxCollider;

    public IScorable scorable;
    public int score;
    public int totalScore = 0;

    public GameObject simonSays;
    private void Awake()
    {
        boxCollider = dropBox.GetComponent<Collider>();
    }


    public void OnTriggerEnter(Collider collision)
    {
        score = collision.gameObject.GetComponent<IScorable>().pointValue;
        totalScore += score;
        score = 0;
        if (collision.gameObject.tag == "Bomb")
        {
            simonSays.SetActive(true);
            Debug.Log("Dropped bomb");
        }
        Destroy(collision.gameObject);
        //Debug.Log("total Score  = " + totalScore);

    }
}
