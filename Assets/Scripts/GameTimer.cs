using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class GameTimer : MonoBehaviour
{
    public int timerLengthInSec;
    private int setTime;
    private int expectedLength;

    IList<Transform> places = new List<Transform>();

    public Sprite[] sprites = new Sprite[10];
    public Sprite empty;


    public State currentState = State.stopped;
    private State lastState;
    public enum State
    {
        running,
        paused,
        stopped
    }


    private void Awake()
    {
        //the transforms for each sprite on the timer display.
        places = GetComponentsInChildren<Transform>().Skip(1).ToList();

        //store time to reset clock
        setTime = timerLengthInSec;
    }
    private void Start()
    {
        //ClawManager.StartClawTimer += StartTimerCoroutine;
    }

    void Update()
    {
        lastState = currentState;

        //starts
        if (Input.GetKeyDown("q") && currentState == State.stopped)
        {
            currentState = State.running;
            StartCoroutine("Timer");
        }

        //pauses 
        if (Input.GetKeyDown("p") && currentState == State.running)
        {
            currentState = State.paused;
            StopCoroutine("Timer");
        }

        //resumes
        if (Input.GetKeyDown("r") && currentState == State.paused)
        {
            currentState = State.running;
            StartCoroutine("Timer");
        }

        //stops when outta time
        if(currentState == State.running && timerLengthInSec <= 0) 
        {
            currentState = State.stopped;
            StartCoroutine(ResetTimer());
        }

    }

    IEnumerator Timer()
    {
        while (timerLengthInSec > 0)
        {
            float elapsedTime = 0;
            IntToSprite();
            while (elapsedTime <= 1)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            
            timerLengthInSec -= 1;
            
        }
        if (timerLengthInSec == 0)
        {
            IntToSprite();
        }

    }

    //takes an integer from the timerCount, and for each digit in the integer,
    //switches a corresponding sprite to reflect the actual count.
    private void IntToSprite()
    {
        string timerCountString = timerLengthInSec.ToString();
        
        for (int i = 0; i < places.Count; i++)
        {
            //If the string is shorter than the expected length, adds a 0 to the left
            string tempString = timerCountString.PadLeft(places.Count, '0');
            //Gets sprite renderer from the indexed child 
            SpriteRenderer spriteRenderer = transform.GetChild(i).GetComponent<SpriteRenderer>();

            //Takes passed string and indexes into it, returning the integer at that index
            //Indexing into a string returns the character as unicode(ASCII)
            //By subtracting the ASCII number for 0(48) from any int 0-9, the integer will be returned as an int
            int dig = tempString[i] - '0';
            Sprite newSprite = sprites[dig];
            spriteRenderer.sprite = newSprite;      
        }
    }



    private IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(1);
        EmptyScore();
        yield return new WaitForSeconds(.5f);
        IntToSprite();
        yield return new WaitForSeconds(.5f);
        EmptyScore();
        timerLengthInSec = setTime;
    }

    private void EmptyScore()
    {
        foreach (Transform t in places)
        {
            SpriteRenderer spriteRenderer = t.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = empty;
            
        }
    }

}
