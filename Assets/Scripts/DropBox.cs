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
    private void Awake()
    {
        boxCollider = dropBox.GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider collision)
    {
        score = collision.gameObject.GetComponent<IScorable>().pointValue;
        totalScore += score;
        score = 0;
        Destroy(collision.gameObject);
        Debug.Log("total Score  = " + totalScore);

    }
}
