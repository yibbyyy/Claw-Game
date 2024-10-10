using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMode : MonoBehaviour
{
    public float alienAGoGo = 0;
    private float alienMax = 100; 
    public float alienGrowin;
    public float alienShrinkage;
    
    public DropBox dropBox;
    public float ufoBonus;

    public GameTimer gameTimer;

    public GameObject alienGoo;
    private float alienGooHome;
    
    
    public State state = State.stalled;
    public enum State
    {
        empty,
        stalled,
        running,
        full,
        resetting

    }

    private void Awake()
    {
        alienGooHome = alienGoo.transform.position.y;
    }

    private void Start()
    {
        GameController.Setup += SetupAlien;
    }
    void Update()
    {
        ufoBonus = dropBox.alienValue;

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

        if (ufoBonus > 0 && state != State.full)
        {
            Debug.Log("ufoBonus = " + ufoBonus);
            alienAGoGo += ufoBonus;
            alienGoo.transform.Translate(Vector3.up * alienGrowin * ufoBonus);
            ufoBonus = 0;
            dropBox.alienValue = 0;
        }

        
        if (alienAGoGo >= alienMax)
        {
            Debug.Log("alien A GO TO 100!!!");
            state = State.full;
        }



    }

    IEnumerator ResetGoo()
    {
        while (alienGoo.transform.position.y >= alienGooHome)
        {
            state = State.resetting;
            alienGoo.transform.Translate(Vector3.down * alienShrinkage * Time.deltaTime);
            alienAGoGo = 0;
            yield return null;
        }

        if (alienGoo.transform.position.y <= alienGooHome)
        {
            state = State.empty;
        }

    }

    void SetupAlien() 
    {
        ResetGoo();
        GameController.Setup -= SetupAlien;
    }
}
