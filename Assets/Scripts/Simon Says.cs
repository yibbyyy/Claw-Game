using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimonSays : MonoBehaviour
{
    //public GameObject up, down, left, right;
    bool startSequence = false;
    public List<GameObject> gameObjectList= new List<GameObject>();
    private List<GameObject> sequence = new List<GameObject>();
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
        
        // Accept input from mouseclick on buttons and add click to input list

        // After adding click to input list check with sequence index if wrong blow up
        // If right, move index and accept input
    }


}
