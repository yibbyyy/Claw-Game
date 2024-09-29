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
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartTimer(roundLength));
            Debug.Log("started Coroutine");
        }
        if (controllable) { Debug.Log("controllable"); };
        
    }


    IEnumerator StartTimer(int roundLength)
    {
        controllable = true;
        yield return new WaitForSeconds(roundDeltaTime);
        roundLength -= roundDeltaTime;
        Debug.Log("roundLength = " + roundLength);

        if (roundLength > 0) { StartCoroutine(StartTimer(roundLength)); }
        else
        {
            controllable = false;
            roundLength = 5;

        }
    }

}
