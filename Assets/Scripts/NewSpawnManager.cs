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
    private string ufo = "UFO";
    private int ufoWeight= 1;

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
        spawnables.Add(ufo, ufoWeight);

        allSpawnables.AddRange(spawnables.Keys);

        lootSpawnables.Add(aBomb);
        lootSpawnables.Add(gBar);
        lootSpawnables.Add(chest);
        lootSpawnables.Add(key);
        lootSpawnables.Add(clock);
        lootSpawnables.Add(ufo);

        coinSpawnables.Add(yCoin);
        coinSpawnables.Add(gCoin);

        for (int i = 0; i < this.transform.childCount; i++) 
        {
            spawnTransforms.Add(this.transform.GetChild(i).transform);
        }


        maxLowerDisplacement = dropBoxWalls.transform.position.y - 2.5f;
        maxUpperDisplacement = dropBoxWalls.transform.position.y;


        GameController.Setup += FirstFill;
    }
    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && currentState == State.empty)
        {
            currentState = State.treasureFill;
            FirstFill();
        }
        if (dropBoxWalls.transform.position.y > maxLowerDisplacement && currentState == State.lowerWalls)
        {
            lowerDropBoxWalls();
        }
        if (dropBoxWalls.transform.position.y <= maxUpperDisplacement && currentState == State.raiseWalls)
        {
            currentState = State.filled;
        }

    }


    public void FirstFill()
    {
        StartCoroutine(FillLoot());
        GameController.Setup -= FirstFill;
    }


    void spawnCoins()
    {
        foreach (Transform t in spawnTransforms)
        {
            string spawnable = coinSpawnables[Random.Range(0, coinSpawnables.Count)];
            Vector3 randomRotation = new Vector3 (Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
            pooling.SpawnFromPool(spawnable, t.position, Quaternion.Euler(randomRotation));
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
            else if (spawnable != aBomb) 
            {
                pooling.SpawnFromPool(spawnable, t.position, Quaternion.Euler(0, 0, 90));
            }
        }
    }


    IEnumerator FillCoins()
    {
        while (refill < refillCounts)
        {
            spawnCoins();
            yield return new WaitForSeconds(waitBetweenRefills);
            refill++;
        }

        if (refill >= refillCounts)
        {
            refill = 0;
            currentState = State.lowerWalls;
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

        if (lootRefill >= lootRefillCount)
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