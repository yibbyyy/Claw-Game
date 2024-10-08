using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireCut : GenericBomb
{
    //public GameObject up, down, left, right;
    public GameObject SucessText;
    public float startDelay = 1f;
    public float timerDuration = 5f;
    public float delayBetweenbuttons = .5f;
    public float disposeDelay = .5f;
    
    public List<Color> colorList = new List<Color>();
    public List<Color> selectedColorsSequence = new List<Color>();
    public HashSet<Color> colorSet = new HashSet<Color>();
    public List<GameObject> gameObjectList = new List<GameObject>();
    //public List<GameObject> stickyNoteSprites = new List<GameObject>();
    public List<Sprite> wireCutSpriteList = new List<Sprite>();
    public List<Sprite> ogWireSprites = new List<Sprite>();
    public Dictionary<GameObject, float> pitchForButtons = new();

    public Sprite orangeSprite, greenSprite, yellowSprite, redSprite, blueSprite, pinkSprite;

    public List<Color> userList = new();
    public Sprite pressedSprite;
    public int wireCutNum = 0;
    private AudioSource audioSource;
    public bool win = false;

    readonly Color orange = new Color(255, 150, 0, 255);
    readonly Color pink = new Color(255, 0, 255, 255);
    protected State currentState = State.playingSequence;

    Dictionary<GameObject, Sprite> wireCutSprites = new();

    private void OnEnable()
    {
        Dictionary<Color, Sprite> colorToSprite = new()
        {
            {orange,  orangeSprite},
            {Color.green, greenSprite},
            {Color.yellow, yellowSprite},
            {Color.red, redSprite},
            {Color.blue, blueSprite},
            {pink, pinkSprite}
        };

        // Map gameobjects to their respective cut wire sprite
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            if (!wireCutSprites.ContainsKey(gameObjectList[i].gameObject))
                wireCutSprites.Add(gameObjectList[i], wireCutSpriteList[i]);
        }
        win = false;
        bombTimerUI.text = bombTimerDuration.ToString("F2");
        audioSource = GetComponent<AudioSource>();
        // Pick between 6 colors to select 4 to be wire sequence
        
        while (selectedColorsSequence.Count < 4)
        {
            int colorIndex = Random.Range(0, colorList.Count);
            if (!colorSet.Contains(colorList[colorIndex]))
            {
                colorSet.Add(colorList[colorIndex]);
                selectedColorsSequence.Add(colorList[colorIndex]);
            }
        }
        foreach (Color color in selectedColorsSequence)
        {
            Debug.Log(color.ToString());
        }

        Assert.AreEqual(selectedColorsSequence.Count, gameObjectList.Count);
        // Now we have a random sequence of 4 colors

        // Randomize the sequence of 4 colors so we can assign a color randomly to wires
        List<Color> randomizedColors = ShuffleColors(selectedColorsSequence);
        for ( int i = 0; i < gameObjectList.Count; i++)
        {
            gameObjectList[i].GetComponent<Image>().color = randomizedColors[i];
        }

        // Also set the sticky note sprites to the right sequence

        StartCoroutine(Timer(timerDuration));

        //Debug.Log("Sequence Length: " +  sequenceLen);






    }

    // Fisher-Yates Shuffle Algorithm to shuffle colors
    List<Color> ShuffleColors(List<Color> availableColors)
    {
        List<Color> shuffledColors = new List<Color>(availableColors);
        int n = shuffledColors.Count;
        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(i, n);
            Color temp = shuffledColors[i];
            shuffledColors[i] = shuffledColors[randomIndex];
            shuffledColors[randomIndex] = temp;
        }
        return shuffledColors;
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


    
        
    

    
    public void ClickedButton()
    {
        //Debug.Log("Clicked Button was called by: " + EventSystem.current.currentSelectedGameObject.name);
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null)
        {
            // Remove interactibility and listener for wire
            clickedButton.GetComponent<Button>().interactable = false;
            clickedButton.GetComponent<Button>().onClick.RemoveListener(ClickedButton);

            // Change sprite to cut wire
            clickedButton.GetComponent<Image>().sprite = wireCutSprites[clickedButton];
            userList.Add(clickedButton.GetComponent<Image>().color);
            if (userList[wireCutNum] != selectedColorsSequence[wireCutNum])
            {
                //Debug.Log("Button press num: " + buttonPressNum);
                //Debug.Log("Incorrect button user: " + userList[buttonPressNum] + " Actual: " + sequence[buttonPressNum]);
                if (!exploded)
                {
                    //Debug.Log("Bomb blows up");
                    StartCoroutine(BombExplosion());
                }

            }


            else if (wireCutNum >= selectedColorsSequence.Count - 1)
            {
                // Play success sound feedback
                Debug.Log("Succesful Defuse");
                StopAllCoroutines();
                // Can fire event and/or just change exploded to true so timer doesn't go off
                win = true;
                exploded = true;

                // Play sucess sound

                Dispose();
            }
            wireCutNum++;
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

    IEnumerator DisposeDelay()
    {
        float duration = 0f;

        while (duration < disposeDelay)
        {
            duration += Time.deltaTime;
            yield return null;
        }

        if (win)
        {
            audioSource.Play();
            SucessText.SetActive(true);
            duration = 0f;

            while (duration < 2.5f)
            {
                duration += Time.deltaTime;
                yield return null;
            }
            SucessText.SetActive(false);
        }
        userList.Clear();
        
        wireCutNum = 0;
        selectedColorsSequence.Clear();
        exploded = false;
        base.Dispose();
    }

    // Call dispose from othe r file
    public override void Dispose()
    {
        bombTimerUI.text = bombTimerDuration.ToString("F2");
        ToggleInteractibility(false);
        ToggleButtonSubscription(false);
        StartCoroutine(DisposeDelay());

    }
}
