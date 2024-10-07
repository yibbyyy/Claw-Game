using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;


public class GameTimer : MonoBehaviour
{
    public GameObject timerDisplay;
    public int timerLengthInSec;
    private int expectedLengh;

    public Object[] sprites;
    IList<Transform> places = new List<Transform>();

    public Sprite[] sprites2 = new Sprite[10];

    private State currentState = State.stopped;
    private enum State
    {
        started,
        paused,
        stopped
    }


    private void Awake()
    {
        sprites = Resources.LoadAll("Digits", typeof(Texture2D));
        places = timerDisplay.GetComponentsInChildren<Transform>();

        expectedLengh = places.Count - 1;
        
        
        foreach(var sprite in sprites)
        {
            Debug.Log("sprite = " + sprite.name);
        }

    }
    private void Start()
    {
        ClawManager.StartClawTimer += StartTimerCoroutine;
    }

    void Update()
    {

        if (Input.GetKeyDown("q") && currentState == State.stopped )
        {
            currentState = State.started;
            StartCoroutine(Timer());
        }
    }

    public void StartTimerCoroutine()
    {
        Debug.Log("started coroutine");
        StartCoroutine(Timer());
    }

    public void PauseTimer()
    {
        StopCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        while (timerLengthInSec > 0)
        {
            ConvertTime();
            yield return new WaitForSeconds(1);
            timerLengthInSec -= 1;
            
        }

    }


    private void ConvertTime()
    {
        string timerCountString = timerLengthInSec.ToString();
        
        for (int i = 0; i < places.Count - 1; i++)
        {

            string tempString = timerCountString.PadLeft(expectedLengh, '0');

            //Gets sprite renderer from the indexed child 
            SpriteRenderer spriteRenderer = timerDisplay.transform.GetChild(i).GetComponent<SpriteRenderer>();

            //Takes passed string and indexes into it, returning the integer at that index
            int dig = tempString[i] - '0';      //Fucking cool ass trick please dont forget about this + look into how it actually works again 

            //Debug.Log("timerCountString = " + timerCountString+ " | i = " + i + " | dig = " + dig);

            //on the chopping block we will see.
            Sprite newSprite = sprites2[dig];
            Debug.Log("dig = " + dig + " | transform = " + transform + " | sprite = " + newSprite);

            spriteRenderer.sprite = newSprite;
      
        }
    }
}
