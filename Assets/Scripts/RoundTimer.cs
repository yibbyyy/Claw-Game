using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public int roundDeltaTime = 1;
    public int roundLength = 5;
    public bool controllable;
    private void Awake()
    {
        controllable = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controllable == false)
        {
            StartCoroutine(StartTimer(roundLength));
            //Debug.Log("started Coroutine");
        }
        if (controllable) { 
            //Debug.Log("controllable");
        };
        
    }


    IEnumerator StartTimer(int roundLength)
    {
        controllable = true;
        yield return new WaitForSeconds(roundLength);

        // Player hasn't turned magnet on
        if (controllable == true) 
        {
            controllable = false;
            // turn magnet on
            // reset arm
        }
        
        // Can't move arm, can't turn on/off magnet, magnet turns on,
        // if already on this timer does nothing
        //Debug.Log("roundLength = " + roundLength);

        
    }

}
