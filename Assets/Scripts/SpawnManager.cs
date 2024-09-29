using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public ObjectPooling pooling;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            
            pooling.SpawnFromPool("Gold Coin", transform.position,Quaternion.identity);
        }
    }
}
