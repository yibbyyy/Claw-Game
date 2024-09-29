using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimonSays : MonoBehaviour
{
    //public GameObject up, down, left, right;
    public float startDelay = 1f;
    public float delayBetweenbuttons = .5f;
    public bool playSequence = true;
    public bool autoSequenceStarted = false;
    
    public List<GameObject> gameObjectList= new List<GameObject>();
    private List<GameObject> sequence = new List<GameObject>();
    public Sprite pressedSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Gameobject list " +  gameObjectList.Count);
        // Randomize sequence length of 4 - 10
        int sequenceLen = Random.Range(4, 10);
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

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Simon says update happening");

        // Play sequence by doing button click animation
        // Need gameobject button component
        if (playSequence && !autoSequenceStarted)
        {
            Debug.Log("Starting sequence");
            autoSequenceStarted = true;
            StartCoroutine(PlaySequence());
            
        }
        // Accept input from mouseclick on buttons and add click to input list

        // After adding click to input list check with sequence index if wrong blow up
        // If right, move index and accept input
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
        playSequence = false;
        autoSequenceStarted = false;
    }
}
