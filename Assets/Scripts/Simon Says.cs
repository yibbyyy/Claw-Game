using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SimonSays : MonoBehaviour
{
    //public GameObject up, down, left, right;
    public float startDelay = 1f;
    public float timerDuration = 5f;
    public float delayBetweenbuttons = .5f;
    public bool playSequence = true;
    public bool autoSequenceStarted = false;
    public bool canTakeInput = false;
    public List<GameObject> gameObjectList= new List<GameObject>();
    public List<GameObject> userList = new List<GameObject>();
    private List<GameObject> sequence = new List<GameObject>();
    public Sprite pressedSprite;
    public int buttonPressNum = 0;
    public Explosion explosionRef;
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Gameobject list " +  gameObjectList.Count);
        // Randomize sequence length of 4 - 10
        int sequenceLen = Random.Range(4, 5);
        //Debug.Log("Sequence Length: " +  sequenceLen);
        // Pick random buttons 0 - 3
        int buttonIndex;
        for (int i = 0; i < sequenceLen; i++)
        {
            buttonIndex = Random.Range(0, 3);
           // Debug.Log("Button Index: " + buttonIndex);
            sequence.Add(gameObjectList[buttonIndex]);
           
        }
        //Debug.Log("Sequence list: " + sequence.Count);


        

    }

    protected enum State
    {
        playingSequence,
        acceptingInput
    }

    protected State currentState = State.playingSequence;
    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Simon says update happening");

        // Play sequence by doing button click animation
        // Need gameobject button component
        if (currentState == State.playingSequence)
        {
            StartCoroutine(PlaySequence());
        }
        
        else if (currentState == State.acceptingInput)
        {
            // Handled by events
        }
       
    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(startDelay);

        for(int i  = 0; i < sequence.Count; i++)
        {
            Debug.Log(sequence[i].name);
            Sprite tmp = sequence[i].GetComponent<Button>().image.sprite;
            sequence[i].GetComponent<Button>().image.sprite = pressedSprite;

            yield return new WaitForSeconds(delayBetweenbuttons);
            sequence[i].GetComponent<Button>().image.sprite = tmp;
            yield return new WaitForSeconds(delayBetweenbuttons);

        }
        
        // Subscribe ClickedButton() to all button onclick events
        for (int i = 0;i < gameObjectList.Count; i++)
        {
            gameObjectList[i].GetComponent<Button>().onClick.AddListener(ClickedButton);
        }

        ToggleInteractibility(true);
        currentState = State.acceptingInput;
        StartCoroutine(Timer(timerDuration));
    }


    public void ClickedButton()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null )
        {
            userList.Add(clickedButton);
            if (userList[buttonPressNum]  != sequence[buttonPressNum] )
            {
                Debug.Log("Bomb blows up");
            }

            buttonPressNum++;
            if (buttonPressNum >= sequence.Count)
            {
                Debug.Log("End of sequence");

                // Make all buttons not interactible
                for (int i = 0; i < gameObjectList.Count; i++)
                {
                    gameObjectList[i].GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    IEnumerator Timer(float duration)
    {
        float timeremaining =  duration;
        while (timeremaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeremaining--;
        }
        // Play event that bomb blew up
        Debug.Log("ran out of time");
        

        
    }

    private void ToggleInteractibility(bool toggle)
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            gameObjectList[i].GetComponent<Button>().interactable = toggle;
        }
    }

    // Call dispose from othe r file
    public void Dispose()
    {
        userList.Clear();
        sequence.Clear();
        explosionRef.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
