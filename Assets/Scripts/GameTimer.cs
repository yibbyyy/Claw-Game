using System;
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


    public DropBox dropBox;
    

    public static event Action timedOut;



    public State currentState = State.stopped;
    private State lastState;
    public enum State
    {
        running,
        paused,
        stopped
    }
    public void MinusTenSeconds()
    {
        timerLengthInSec -= 10;
    }

    private void Awake()
    {
        //the transforms for each sprite on the timer display.
        places = GetComponentsInChildren<Transform>().Skip(1).ToList();

        //store time to reset clock
        setTime = timerLengthInSec;

        GameController.Setup += SetupTimer;
        GenericBomb.BombExploded += MinusTenSeconds;
    }
    private void Start()
    {
        ClawManager.StartClawTimer += StartGameTimer;
        
        //ClawTimer.ClawTimerEnded += PauseGameTimer;
    }

    void Update()
    {
        //lastState = currentState;

        //Updates if clock is dropped in dropBox
        if (dropBox.timeValue != 0)
        {
            timerLengthInSec += dropBox.timeValue;
            IntToSprite();
            dropBox.timeValue = 0;  
        }


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
    void StartGameTimer()
    {
        currentState = State.running;
        StartCoroutine("Timer");
        
        ClawManager.StartClawTimer -= StartGameTimer;
    }

    /*
    void PauseGameTimer()
    {
        currentState = State.paused;
        
        StopCoroutine("Timer");
        ClawTimer.ClawTimerEnded -= PauseGameTimer;
    }
    */

    IEnumerator Timer()
    {
        while (timerLengthInSec > 0)
        {
            float elapsedTime = 0;
            IntToSprite();
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                //Debug.Log(elapsedTime);
                yield return null;
            }
            
            
            timerLengthInSec -= 1;
            
        }
        if (timerLengthInSec == 0)
        {
            timedOut?.Invoke();
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

    public void SetupTimer()
    {
        Debug.Log("Setup Timer");
        IntToSprite();
        GameController.Setup -= SetupTimer;
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
