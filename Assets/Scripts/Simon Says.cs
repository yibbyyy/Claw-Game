using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SimonSays : GenericBomb
{
    //public GameObject up, down, left, right;
    
    public float startDelay = 1f;
    public float timerDuration = 5f;
    public float delayBetweenbuttons = .5f;
    public List<GameObject> gameObjectList= new List<GameObject>();
    public Dictionary<GameObject, float> pitchForButtons = new();
    public List<GameObject> userList = new List<GameObject>();
    private List<GameObject> sequence = new List<GameObject>();
    public Sprite pressedSprite;
    public int buttonPressNum = 0;
    private AudioSource audioSource;

    protected State currentState = State.playingSequence;
    // Start is called before the first frame update
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        //Debug.Log("Gameobject list " +  gameObjectList.Count);
        // Randomize sequence length of 4 - 10
        int sequenceLen = Random.Range(4, 5);
        //Debug.Log("Sequence Length: " +  sequenceLen);
        // Pick random buttons 0 - 3
        int buttonIndex;
        for (int i = 0; i < sequenceLen; i++)
        {
            buttonIndex = Random.Range(0, 4);
           // Debug.Log("Button Index: " + buttonIndex);
            sequence.Add(gameObjectList[buttonIndex]);
           
        }
        //Debug.Log("Sequence list: " + sequence.Count);
        for (int i = 0;i < sequenceLen; i++)
        {
            Debug.Log(i + " " + sequence[i].name);
        }
        // Play sequence
        StartCoroutine(PlaySequence());



    }

    protected enum State
    {
        playingSequence,
        acceptingInput
    }

    
    // Update is called once per frame
    void Update()
    {

        
        if (currentState == State.playingSequence)
        {
            // Handled in start
        }
        
        else if (currentState == State.acceptingInput)
        {
            // Handled by events
        }
       
    }

   
    IEnumerator PlaySequence()
    {
        float duration = 0;
        while (duration < startDelay)
        {
            duration += Time.deltaTime;
            yield return null;
        }
        

        for(int i  = 0; i < sequence.Count; i++)
        {
            // Pressing button
            Debug.Log(sequence[i].name);
            Sprite tmp = sequence[i].GetComponent<Button>().image.sprite;
            sequence[i].GetComponent<Button>().image.sprite = pressedSprite;

            // Play Sound
            sequence[i].GetComponent<AudioSource>().Play();
            duration = 0;
            while (duration < delayBetweenbuttons)
            {
                duration += Time.deltaTime;
                yield return null;
            }

            sequence[i].GetComponent<Button>().image.sprite = tmp;
            // Releasing button
            duration = 0;
            while (duration < delayBetweenbuttons)
            {
                duration += Time.deltaTime;
                yield return null;
            }

        }

        ToggleButtonSubscription(true);
       

        ToggleInteractibility(true);
        currentState = State.acceptingInput;
        StartCoroutine(Timer(timerDuration));
    }


    public void ClickedButton()
    {
        //Debug.Log("Clicked Button was called by: " + EventSystem.current.currentSelectedGameObject.name);
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null )
        {
            userList.Add(clickedButton);
            if (userList[buttonPressNum]  != sequence[buttonPressNum] )
            {
                //Debug.Log("Button press num: " + buttonPressNum);
                //Debug.Log("Incorrect button user: " + userList[buttonPressNum] + " Actual: " + sequence[buttonPressNum]);
                if (!exploded)
                {
                    //Debug.Log("Bomb blows up");
                    StartCoroutine(BombExplosion());
                }
                
            }

            buttonPressNum++;
            if (buttonPressNum >= sequence.Count)
            {
                Debug.Log("Succesful Defuse");
                // Can fire event and/or just change exploded to true so timer doesn't go off
                exploded = true;

                // Make all buttons not interactible
                
                Dispose();
            }
        }
    }

    
    private void ToggleButtonSubscription(bool toggle)
    {
        if (toggle)
        {
            // Subscribe ClickedButton() to all button onclick events
            for (int i = 0; i < gameObjectList.Count; i++)
            {
                gameObjectList[i].GetComponent<Button>().onClick.AddListener(ClickedButton);
            }
        }

        else
        {
            // Unsubscribe ClickedButton() to all button onclick events
            for (int i = 0; i < gameObjectList.Count; i++)
            {
                gameObjectList[i].GetComponent<Button>().onClick.RemoveListener(ClickedButton);
            }
        }
    }
    private void ToggleInteractibility(bool toggle)
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            gameObjectList[i].GetComponent<Button>().interactable = toggle;
        }
    }

    // Call dispose from othe r file
    public override void Dispose()
    {
        bombTimerUI.text = bombTimerDuration.ToString("F2");
        ToggleButtonSubscription(false);
        ToggleInteractibility(false);
        userList.Clear();
        buttonPressNum = 0;
        sequence.Clear();
        exploded = false;
        base.Dispose();
    }
}
