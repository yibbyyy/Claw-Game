using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameTimer : MonoBehaviour
{
    public int timerLengthInSec;
    private int expectedLength;

    //public Object[] sprites;
    IList<Transform> places = new List<Transform>();

    public Sprite[] sprites2 = new Sprite[10];

    private State currentState = State.stopped;
    private State lastState;
    private enum State
    {
        running,
        paused,
        stopped
    }


    private void Awake()
    {
        //There might be a way to cast this correctly. If needed. could be useful in future circumstances
        //sprites = Resources.LoadAll("Digits", typeof(Texture2D));

        //the transforms for each sprite on the timer display.
        places = GetComponentsInChildren<Transform>().Skip(1).ToList();
        //IF THE THING BELOW THIS GETS USED, -1 FROM THE CONVERTER
        //expectedLength = places.Count - 1;
        Debug.Log("expected = " + places.Count);
    }
    private void Start()
    {
        //ClawManager.StartClawTimer += StartTimerCoroutine;
    }

    void Update()
    {
        if (currentState != lastState) { Debug.Log("State changed to = " + currentState); }
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
            currentState= State.running;
            StartCoroutine("Timer");
        }

    }

    IEnumerator Timer()
    {
        while (timerLengthInSec > 0)
        {
            IntToSprite();
            yield return new WaitForSeconds(1);
            timerLengthInSec -= 1;
            
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

            Sprite newSprite = sprites2[dig];
            spriteRenderer.sprite = newSprite;      
        }
    }


    /*  For use when we create the event system with claw timer and alien mode
     *  Dumb Thought For The Day (DTFTD) Imagine a big system of states in one 
     *  behemoth script, with attached coroutines. or perhaps a network of states 
     *  across scripts. For the Game Controller, we use bitflag combinations to run 
     *  a more instructed set of functions via the respective state combos
     *  ~Source~
     *  https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
    public void StartTimerCoroutine()
    {
        Debug.Log("started coroutine");
        StartCoroutine(Timer());
    }


    */

    private void ResetTimer()
    {
        
    }

}
