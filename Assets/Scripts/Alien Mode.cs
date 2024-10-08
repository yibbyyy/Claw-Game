using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMode : MonoBehaviour
{
    public float alienAGoGo = 0;
    private float alienMax = 100; 
    public float alienGrowin; 

    public GameTimer gameTimer;

    public GameObject alienGoo;
    
    public State state = State.stalled;
    public enum State
    {
        stalled,
        running
    }


    void Update()
    {
        
        if (gameTimer.currentState == GameTimer.State.running)
        {
            state = State.running;
        }
        if (gameTimer.currentState != GameTimer.State.running)
        {
            state = State.stalled;
        }

        if (state == State.running)
        {
            alienGoo.transform.Translate(Vector3.up * alienGrowin * Time.deltaTime);
            alienAGoGo += Time.deltaTime;

        }

        
        if (alienAGoGo >= alienMax)
        {
            Debug.Log("alien A GO TO 100!!!");
            state = State.stalled;
        }
    }


}
