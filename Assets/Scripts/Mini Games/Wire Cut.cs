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
    public float disposeDelay = .5f;
    
    public List<Color> colorList = new List<Color>();
    public HashSet<Color> colorSet = new HashSet<Color>();

    
    public List<GameObject> gameObjectList = new List<GameObject>();
    public List<GameObject> stickyNoteStars = new();
    public List<Sprite> wireCutSpriteList = new List<Sprite>();
    public List<Sprite> ogWireSprites = new List<Sprite>();

    public HashSet<Color> userSet = new();
    Dictionary<GameObject, Sprite> wireCutSprites = new();

    public int wireCutNum = 0;
    private AudioSource audioSource;
    public bool win = false;
    public bool isAlienBomb = false;
    Color yellow = new(1, 1, 0, 1);

    int wiresNeedCutLen;

    Dictionary<Color, GameObject> ColorToAlienStars;
        


    private void OnEnable()
    {
        
        Dictionary<Color, GameObject> ColorToStars = new()
        {
            {Color.red, stickyNoteStars[0]},
            {Color.blue, stickyNoteStars[1]},
            {Color.green, stickyNoteStars[2]},
            {yellow, stickyNoteStars[3]}
        };

        ColorToAlienStars = new()
        {
            {Color.red, stickyNoteStars[4]},
            {Color.blue, stickyNoteStars[5]},
            {Color.green, stickyNoteStars[6]},
            {yellow, stickyNoteStars[7]}
        };

        /*
        foreach (Color c in ColorToStars.Keys)
        {
            Debug.Log("Key is: " + c + "Value is: " + ColorToStars[c]);
        }
        */
        // Map gameobjects to their respective cut wire sprite
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            if (!wireCutSprites.ContainsKey(gameObjectList[i].gameObject))
                wireCutSprites.Add(gameObjectList[i], wireCutSpriteList[i]);
        }
        win = false;
        bombTimerUI.text = bombTimerDuration.ToString("F0");
        audioSource = GetComponent<AudioSource>();


        // Pick how many wires need to be cut

        wiresNeedCutLen = Random.Range(1, 4);


        while (colorSet.Count < wiresNeedCutLen)
        {
            Color c = colorList[Random.Range(0, 4)];

            if (!colorSet.Contains(c))
            {
                colorSet.Add(c);
            }
        }
        /*
        foreach (Color color in colorSet)
        {
            Debug.Log(color.ToString());
        }
        */
        
        //Now we have a random number of colors with a random sequence

        
        

        // Also set the sticky note star sprites to the right sequence
        if (!isAlienBomb)
        {
            foreach (Color color in colorSet)
            {
                ColorToStars[color].SetActive(true);
            }
        }
       
        

        StartCoroutine(Timer(timerDuration));
        ToggleInteractibility(true);
        ToggleButtonSubscription(true);
        






    }

    
   

    


    
        
    
    public void drawAlienStars()
    {
        Debug.Log($"Drew alien stars:");
        foreach (Color color in colorSet)
        {
            ColorToAlienStars[color].SetActive(true);
            Debug.Log($"Color: {color} was selected");
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

            // Check if the color of wire is in the color sequence
            if (colorSet.Contains(clickedButton.GetComponent<Image>().color))
            {
                
                userSet.Add(clickedButton.GetComponent<Image>().color);
                if (userSet.SetEquals(colorSet))
                {
                    // Defuse successful!
                    StopAllCoroutines();
                    // Can fire event and/or just change exploded to true so timer doesn't go off
                    win = true;
                    exploded = true;

                    

                    Dispose();
                }
            }
            else
            {
                Debug.Log($"ColorSet doesn't contain {clickedButton}'s color of {clickedButton.GetComponent<Image>().color}");
                if (!exploded)
                {
                    //Debug.Log("Bomb blows up");
                    StartCoroutine(BombExplosion());
                }
            }
            
        }
    }

    private void ToggleStars(bool toggle)
    {
            for (int i = 0; i < gameObjectList.Count; i++)
            {
                stickyNoteStars[i].SetActive(toggle);
            }
       
    }

    
    private void ToggleAlienStars(bool toggle)
    {
        foreach( GameObject v in ColorToAlienStars.Values)
        {
            v.SetActive(toggle);
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

    private void CleanUpWires()
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            gameObjectList[i].GetComponent<Image>().sprite = ogWireSprites[i];
            gameObjectList[i].SetActive(true);
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
        userSet.Clear();
        colorSet.Clear();
        CleanUpWires();
        // Set wires to be og sprites and set them to be active
        
        
        exploded = false;
        base.Dispose();
    }

    // Call dispose from othe r file
    public override void Dispose()
    {
        bombTimerUI.text = bombTimerDuration.ToString("F0");
        ToggleInteractibility(false);
        ToggleButtonSubscription(false);
        ToggleStars(false);
        ToggleAlienStars(false);
        StartCoroutine(DisposeDelay());

    }
}
