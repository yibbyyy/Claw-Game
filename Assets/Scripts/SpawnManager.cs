using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public ObjectPooling pooling;


    private string gCoin = "Gold Coin";
    private float gCoinWeight = 1f;
    private string sCoin = "Silver Coin";
    private float sCoinWeight = .5f;

    private Dictionary<string, float> spawnables = new Dictionary<string, float>();
    private IList<string> strings = new List<string>();

    private void Awake()
    {
        //Hand Build the dictionary oofers
        spawnables.Add(gCoin, gCoinWeight);
        spawnables.Add(sCoin, sCoinWeight);

        strings.AddRange(spawnables.Keys);

    }

    void Update()
    {


        if (Input.GetMouseButtonDown(2))
        {

            spawnSeveral();

        }
    }

    void spawnSeveral()
    {
        string spawnable = strings[Random.Range(0, strings.Count - 1)];
        float spawnCount = Random.Range(0, 100) * spawnables[spawnable];

        Debug.Log("spawnable = " + spawnable + " | spawn count = " + spawnCount);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 displacement = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.1f, 0.1f), Random.Range(-0.25f, 0.25f));
            pooling.SpawnFromPool(spawnable, transform.position + displacement, Quaternion.identity);
        }
    }



}
