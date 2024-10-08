using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossChecking : MonoBehaviour
{
    public GameObject timerDisplay;
    public int timerLengthInSec;
    int expectedLengh;
    float duration = 0;

    //public Texture2D[] sprites;
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
        //sprites = (Texture2D[])Resources.LoadAll("Digits", typeof(Texture2D));
        places = timerDisplay.GetComponentsInChildren<Transform>();

        expectedLengh = places.Count - 1;
    }

    private void Start()
    {
        ClawManager.StartClawTimer += StartTimerCoroutine;
    }

    void Update()
    {
        //swapped
        if (Input.GetKeyDown("q") && currentState == State.stopped)
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
            timerLengthInSec -= 1;
            while (duration < 1f)
            {
                duration += Time.deltaTime;
                yield return null;
            }
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

            Sprite newSprite = sprites2[dig];
            spriteRenderer.sprite = newSprite;

        }
    }
}
