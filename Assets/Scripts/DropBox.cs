using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropBox : MonoBehaviour
{
    public GameObject dropBox;
    

    public ObjectPooling pooling;
    public IScorable scorable;
    public int score;
    public int totalScore = 0;


    public AlienMode AlienMode;
    public float alienValue;

    public GameTimer gameTimer;
    public int timeValue;


    public GameObject simonSays, wireCut, pushAndPull;
    public Sprite humanBomb, alienBomb;

    private List<GameObject> miniGameList = new();

    public Queue<GameObject> dropBoxQueue = new();
    public HashSet<int> gameObjectInstances = new HashSet<int>();

    
    private void Awake()
    {
        miniGameList.Add(simonSays);
        miniGameList.Add(wireCut);
        miniGameList.Add(pushAndPull);
        
    }


    private void Update()
    {
        // TODO add check to see if chest logic is playing
        // TODO add logic to stop the game timer while playing mingames and chest animations
        if (dropBoxQueue.Count > 0 && !simonSays.activeSelf && !wireCut.activeSelf && !pushAndPull.activeSelf)
        {
            GameObject currentObject = dropBoxQueue.Dequeue();
            int id = currentObject.GetInstanceID();
            gameObjectInstances.Remove(id);
            int miniGameIndex;
            switch (currentObject.tag)
            {
                case "Bomb":
                    miniGameIndex = Random.Range(0, miniGameList.Count);
                    miniGameList[miniGameIndex].SetActive(true);
                    miniGameList[miniGameIndex].GetComponent<GenericBomb>().SwitchBombSprite(humanBomb);
                    break;

                case "ABomb":
                    miniGameIndex = Random.Range(0, miniGameList.Count);
                    miniGameList[miniGameIndex].SetActive(true);
                    miniGameList[miniGameIndex].GetComponent<GenericBomb>().SwitchBombSprite(alienBomb);
                    break;
                case "Chest":
                    Debug.Log("Chest logic");
                    break;

                case "Key":
                    Debug.Log("Key Logic");
                    break;


                default:
                    Debug.Log(currentObject.tag + " Is not an item with a tag");
                    break;

            }
        }
    }
    public void OnTriggerEnter(Collider collision)
    {
        score = collision.gameObject.GetComponent<IScorable>().pointValue;
        totalScore += score;
        score = 0;


        if (collision.gameObject.GetComponent<IScorable>().alienValue > 0) 
        {
            alienValue = collision.gameObject.GetComponent<IScorable>().alienValue;
            Debug.Log("alienValue = " + alienValue);
            
        }
        
        if (collision.gameObject.GetComponent<IScorable>().timeValue != 0) 
        {
            timeValue = collision.gameObject.GetComponent<IScorable>().timeValue;
        }


        timeValue = collision.gameObject.GetComponent <IScorable>().timeValue;


        // Check if a chest or bomb fell
        if (collision.gameObject.tag == "Bomb" || collision.gameObject.tag == "Chest" || collision.gameObject.tag == "ABomb" || collision.gameObject.tag == "Key")
        {
            // Check if the current collision is already in queue using hashset
            int tmp = collision.gameObject.GetInstanceID();
            if (!gameObjectInstances.Contains(tmp))
            {
                gameObjectInstances.Add(tmp);
                dropBoxQueue.Enqueue(collision.gameObject);
            }
            
        }
        // Send the game object back to object pool

        collision.gameObject.SetActive(false);
        //Destroy(collision.gameObject);
        //Debug.Log("total Score  = " + totalScore);

    }
}
