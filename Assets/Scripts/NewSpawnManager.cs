using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class NewSpawnManager : MonoBehaviour
{
    public GameObject dropBoxWalls;
    public float moveSpeed;
    private float maxUpperDisplacement;
    private float maxLowerDisplacement;

    public ObjectPooling pooling;
    public Transform spawner;
    /*
    private string yCoin = "YellowCoin";
    private int yCoinWeight = 1;
    private string gCoin = "GreyCoin";
    private int gCoinWeight = 1;
    private string aBomb = "A Bomb";
    private int aBombWeight = 1;
    private string gBar = "New Gold Bar";
    private int gBarWeight = 1;
    private string chest = "Metal Box";
    private int chestWeight = 1;
    private string key = "Key";
    private int keyWeight = 1;
    private string clock = "Clock";
    private int clockWeight = 1;
    */

    private string yCoin = "Big Yellow Coin";
    private int yCoinWeight = 1;
    private string gCoin = "Big Grey Coin";
    private int gCoinWeight = 1;
    private string aBomb = "Big A Bomb";
    private int aBombWeight = 1;
    private string gBar = "Big Gold Bar";
    private int gBarWeight = 1;
    private string chest = "Big Box";
    private int chestWeight = 1;
    private string key = "Big Key";
    private int keyWeight = 1;
    private string clock = "Big Clock";
    private int clockWeight = 1;

    private Dictionary<string, float> spawnables = new Dictionary<string, float>();
    private IList<string> allSpawnables = new List<string>();
    private IList<string> lootSpawnables = new List<string>();
    private IList<string> coinSpawnables = new List<string>();

    private IList<Transform> spawnTransforms = new List<Transform>();

    public int refillCounts;
    public float waitBetweenRefills;
    private int refill = 0;

    public int lootRefillCount;
    public float waitBetweenLootRefills;
    private int lootRefill = 0;

    public Vector3 lootSpawnDirection;

    public State currentState = State.empty;

    public enum State
    {
        empty,
        filled,
        treasureFill,
        coinFill,
        raiseWalls,
        lowerWalls,
        generalFill
    }

    private void Awake()
    {
        //Hand Build the dictionary oofers
        spawnables.Add(yCoin, yCoinWeight);
        spawnables.Add(gCoin, gCoinWeight);
        spawnables.Add(aBomb, aBombWeight);
        spawnables.Add(gBar, gBarWeight);
        spawnables.Add(chest, chestWeight);
        spawnables.Add(key, keyWeight);
        spawnables.Add(clock, clockWeight);

        allSpawnables.AddRange(spawnables.Keys);

        lootSpawnables.Add(aBomb);
        lootSpawnables.Add(gBar);
        lootSpawnables.Add(chest);
        lootSpawnables.Add(key);
        lootSpawnables.Add(clock);

        coinSpawnables.Add(yCoin);
        coinSpawnables.Add(gCoin);


        for (int i = 0; i < this.transform.childCount; i++) 
        {
            spawnTransforms.Add(this.transform.GetChild(i).transform);
        }


        maxLowerDisplacement = dropBoxWalls.transform.position.y;
        maxUpperDisplacement = dropBoxWalls.transform.position.y + 2.25f;



    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && currentState == State.empty)
        {
            currentState = State.treasureFill;
            FirstFill();
        }
        if (dropBoxWalls.transform.position.y < maxUpperDisplacement && currentState == State.raiseWalls)
        {
            raiseDropBoxWalls();
        }
        if (dropBoxWalls.transform.position.y >= maxUpperDisplacement && currentState == State.raiseWalls)
        {
            currentState = State.filled;
        }

    }


    void FirstFill()
    {
        StartCoroutine(FillLoot());
    }


    void spawnCoins()
    {
        foreach (Transform t in spawnTransforms)
        {
            string spawnable = coinSpawnables[Random.Range(0, coinSpawnables.Count)];
            float spawnCount = Random.Range(1, 10) * spawnables[spawnable];

            Debug.Log("spawnable = " + spawnable + " | spawn count = " + spawnCount + " | spawned at = " + t.position);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 displacement = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                pooling.SpawnFromPool(spawnable, t.position + displacement, Quaternion.identity);
            }
        }
    }

    void spawnLoot()
    {
        foreach (Transform t in spawnTransforms)
        {
            string spawnable = lootSpawnables[Random.Range(0, lootSpawnables.Count)];
            if (spawnable == aBomb)
            {
                pooling.SpawnFromPool(spawnable, t.position, Quaternion.identity);
            }
            if (spawnable == clock)
            {
                pooling.SpawnFromPool(spawnable, t.position, Quaternion.Euler(-90, 0, 0));
            }
            else
            {
                pooling.SpawnFromPool(spawnable, t.position, Quaternion.Euler(0, 0, 90));
            }
        }
    }


    IEnumerator FillCoins()
    {
        while (refill < refillCounts)
        {
            Debug.Log("refill = " + refill + " | refillCounts = " + refillCounts);
            spawnCoins();
            yield return new WaitForSeconds(waitBetweenRefills);
            refill++;
        }

        if (refill >= refillCounts)
        {
            refill = 0;
            currentState = State.raiseWalls;
            raiseDropBoxWalls();
        }
    }

    IEnumerator FillLoot()
    {
        while (lootRefill < lootRefillCount)
        {
            spawnLoot();
            yield return new WaitForSeconds(waitBetweenLootRefills);
            lootRefill++;
        }

        if(lootRefill >= lootRefillCount)
        {
            lootRefill = 0;
            currentState = State.coinFill;
            StartCoroutine(FillCoins());
        }
    }

    void raiseDropBoxWalls()
    {
        dropBoxWalls.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    void lowerDropBoxWalls()
    {
        dropBoxWalls.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}   