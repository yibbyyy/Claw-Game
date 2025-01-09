using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SpawnManager : MonoBehaviour
{
    public ObjectPooling pooling;
    public Transform spawner;

    private string gCoin = "Gold Coin";
    private float gCoinWeight = 1f;
    private string sCoin = "Silver Coin";
    private float sCoinWeight = .75f;
    private string aBomb = "Alien Bomb";
    private float aBombWeight = .05f;

    private Dictionary<string, float> spawnables = new Dictionary<string, float>();
    private IList<string> strings = new List<string>();

    public int refillCounts;
    private int refill = 0;

    public float moveSpeed = 1;
    private bool isMovingFirst = false;
    private bool isMovingSecond = false;
    private bool isMovingThird = false;
    private bool firstStep = false;
    private bool secondStep = false;
    private bool thirdStep = false;

    private void Awake()
    {
        //Hand Build the dictionary oofers
        spawnables.Add(gCoin, gCoinWeight);
        spawnables.Add(sCoin, sCoinWeight);
        spawnables.Add(aBomb, aBombWeight);


        strings.AddRange(spawnables.Keys);                         
        foreach(var key in spawnables)
        {
            Debug.Log("key = " + key);
        }
        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Starting FUll");
            firstStep = true;
            StartCoroutine(FillBasin());
        }

        if (!isMovingFirst && firstStep)
        {
            Debug.Log("first step");
            isMovingFirst = true;
            StartCoroutine(MoveXPos());
        }
        if (!isMovingSecond && secondStep)
        {
            Debug.Log("second step");
            isMovingSecond = true;
            StartCoroutine(MoveXNeg());
        }
        if (!isMovingThird && thirdStep)
        {
            Debug.Log("third step");
            isMovingThird = true;
            StartCoroutine(MoveCenter());
        }
    }

    void spawnSeveral()
    {
        string spawnable = strings[Random.Range(0, strings.Count)];
        float spawnCount = Random.Range(25,50) * spawnables[spawnable];

        Debug.Log("spawnable = " + spawnable + " | spawn count = " + spawnCount);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 displacement = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.1f, 0.1f), Random.Range(-0.25f, 0.25f));
            pooling.SpawnFromPool(spawnable, spawner.position + displacement, Quaternion.identity);
        }
    }

    IEnumerator FillBasin()
    {
        while (refill < refillCounts)
        {
            spawnSeveral();
            yield return new WaitForSeconds(.05f);
            refill++;
        }
        
    }

    IEnumerator MoveXPos()
    {
        Vector3 offset = transform.position + Vector3.left;
        while (transform.position.x > offset.x) 
        { 
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            yield return null;
        }

        firstStep = false;
        isMovingFirst = false;
        secondStep = true;
    }
    IEnumerator MoveXNeg() 
    {
        Vector3 offset = transform.position + (Vector3.right * 2);
        while (transform.position.x < offset.x)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            yield return null;
        }

        secondStep = false;
        isMovingSecond = false;
        thirdStep = true;
    }
    IEnumerator MoveCenter() 
    {
        Vector3 offset = transform.position + Vector3.left;
        while (transform.position.x > offset.x)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            yield return null;
        }

        thirdStep = false;
        isMovingThird = false;
    }
}
