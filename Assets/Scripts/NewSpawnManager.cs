using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class NewSpawnManager : MonoBehaviour
{
    public ObjectPooling pooling;
    public Transform spawner;

    private string yCoin = "YellowCoin";
    private int yCoinWeight = 1;
    private string gCoin = "GreyCoin";
    private int gCoinWeight = 1;
    private string aBomb = "Alien Bomb";
    private int aBombWeight = 1;
    private string gBar = "New Gold Bar";
    private int gBarWeight = 1;
    private string chest = "Metal Box";
    private int chestWeight = 1;
    private string key = "Key";
    private int keyWeight = 1;
    private string clock = "Clock";
    private int clockWeight = 1;

    private Dictionary<string, float> spawnables = new Dictionary<string, float>();
    private IList<string> allSpawnables = new List<string>();
    private IList<string> lootSpawnables = new List<string>();
    private IList<string> coinSpawnables = new List<string>();

    private IList<Transform> spawnTransforms = new List<Transform>();

    public int refillCounts;
    public float waitBetweenRefills;
    private int refill = 0;

    public Vector3 lootSpawnDirection;

    public enum spawnStates
    {
        idle,
        treasureFill,
        coinFill,
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

        lootSpawnables.Add("Alien Bomb");
        lootSpawnables.Add("New Gold Bar");
        lootSpawnables.Add("Metal Box");
        lootSpawnables.Add("Key");
        lootSpawnables.Add("Clock");

        coinSpawnables.Add("YellowCoin");
        coinSpawnables.Add("GreyCoin");


        for (int i = 0; i < this.transform.childCount; i++) 
        {
            spawnTransforms.Add(this.transform.GetChild(i).transform);
        }


        

        Debug.Log(spawnTransforms.Count);

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Fill Basin");
            //StartCoroutine(FillBasin());
            spawnLoot();
        }
    }


    [ContextMenu("Spawn Somethin")]
    void spawnCoins()
    {

        Debug.Log("spawned");
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
        Debug.Log("Loot Round");
        foreach(Transform t in spawnTransforms)
        {
            string spawnable = lootSpawnables[Random.Range(0, lootSpawnables.Count)];
            pooling.SpawnFromPool(spawnable, t.position, Quaternion.Euler(lootSpawnDirection));
        }
        Debug.Break();

    }


    IEnumerator FillBasin()
    {
        while (refill < refillCounts)
        {
            Debug.Log("refill = " + refill + " | refillCounts = " + refillCounts);
            spawnCoins();
            yield return new WaitForSeconds(waitBetweenRefills);
            refill++;
        }
        

    }

}   