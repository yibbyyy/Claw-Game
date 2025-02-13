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
    private int aBombWeight = 15;
    private string Bomb = "Bomb";
    private int bombWeight = 15;
    private string gBar = "New Gold Bar";
    private int gBarWeight = 20;
    private string chest = "Metal Box";
    private int chestWeight = 5;
    private string key = "Key";
    private int keyWeight = 5;
    private string clock = "Clock";
    private int clockWeight = 20;
    private string ufo = "UFO";
    private int ufoWeight = 20;

    private Dictionary<string, float> spawnables = new Dictionary<string, float>();
    private IList<string> allSpawnables = new List<string>();
    private IList<string> lootSpawnables = new List<string>();
    private IList<string> coinSpawnables = new List<string>();

    private IList<Transform> spawnTransforms = new List<Transform>();

    List<string> spawnTable = new List<string>();


    public int refillCounts;
    public float waitBetweenRefills;
    private int refill = 0;


    public int lootRefillCount;
    public float waitBetweenLootRefills;
    private int lootRefill = 0;

    public StartButton startButton;

    public Vector3 lootSpawnDirection;

    //tracker variables
    [SerializeField]
    private int totalSpawns, gCoins, sCoins, gBars, ufos, clocks, aBombs, hBomb, chests, keys;


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
        spawnables.Add(Bomb, bombWeight);
        spawnables.Add(gBar, gBarWeight);
        spawnables.Add(chest, chestWeight);
        spawnables.Add(key, keyWeight);
        spawnables.Add(clock, clockWeight);
        spawnables.Add(ufo, ufoWeight);

        allSpawnables.AddRange(spawnables.Keys);

        lootSpawnables.Add(aBomb);
        lootSpawnables.Add(Bomb);
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
        foreach (KeyValuePair<string, float> kvp in spawnables) 
        {
            if (kvp.Key != gCoin && kvp.Key != yCoin)
            {
                float spawnWeight = kvp.Value;
                for (int j = 0; j < spawnWeight; j++)
                {
                    spawnTable.Add(kvp.Key);
                }
            }
            Debug.Log($"spawnTablecount = {spawnTable.Count}");
        }
            
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

    void FillSpawnTable()
    {
        foreach (string tag in lootSpawnables)
        {

        }
    }




    public void FirstFill()
    {
        /* Hey we may need to yield return this coroutine
         * so that executation will wait for it to continue before proceeding
         */
        StartCoroutine(FirstFillCoins());
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
            string spawnable = spawnTable[Random.Range(0, spawnTable.Count)];
            if (spawnable == aBomb || spawnable == Bomb)
            {
                pooling.SpawnFromPool(spawnable, t.position, Quaternion.identity);
            }
            else if (spawnable == clock)
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
            spawnCoins();
            yield return new WaitForSeconds(waitBetweenRefills);
            refill++;
        }

        if (refill >= refillCounts)
        {
            startButton.clickable = true;
            refill = 0;
            currentState = State.lowerWalls;
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


    IEnumerator FirstFillCoins()
    {
        while (refill < refillCounts)
        {
            spawnCoins();
            yield return new WaitForSeconds(waitBetweenRefills);
            refill++;
        }

        if (refill >= refillCounts)
        {
            yield return new WaitForSeconds(waitBetweenLootRefills);
            refill = 0;
            currentState = State.treasureFill;
            StartCoroutine(FillLoot());
        }
    }
}