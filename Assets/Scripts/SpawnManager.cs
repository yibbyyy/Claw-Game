using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public ObjectPooling pooling;

    public int goldCoinSpawnMax;
    public int silverCoinSpawnMax;

    
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            for (int i = 0; i < goldCoinSpawnMax; i++)
            {
                Vector3 displacement = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.1f, 0.1f), Random.Range(-0.5f, 0.5f));
                pooling.SpawnFromPool("Gold Coin", transform.position + displacement, Quaternion.identity);
            }
        }
    }
}
