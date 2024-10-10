using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropBox : MonoBehaviour
{
    public GameObject dropBox;
    private Collider boxCollider;

    public IScorable scorable;
    public int score;
    public int totalScore = 0;
    public GameObject simonSays, wireCut, pushAndPull;

    private List<GameObject> miniGameList = new();

    public Queue<GameObject> dropBoxQueue = new();
    public HashSet<int> gameObjectInstances = new HashSet<int>();
    private void Awake()
    {
        miniGameList.Add(simonSays);
        miniGameList.Add(wireCut);
        miniGameList.Add(pushAndPull);
        boxCollider = dropBox.GetComponent<Collider>();
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

            switch (currentObject.tag)
            {
                case "Bomb":
                    int miniGameIndex = Random.Range(0, miniGameList.Count);
                    miniGameList[miniGameIndex].SetActive(true);
                    break;

                case "Chest":
                    Debug.Log("Chest logic");
                    break;

                default:
                    Debug.Log(currentObject.tag + " Is not a bomb or chest");
                    break;

            }
        }
    }
    public void OnTriggerEnter(Collider collision)
    {
        score = collision.gameObject.GetComponent<IScorable>().pointValue;
        totalScore += score;
        score = 0;

        // Check if a chest or bomb fell
        if (collision.gameObject.tag == "Bomb" || collision.gameObject.tag == "Chest")
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

        //Destroy(collision.gameObject);
        //Debug.Log("total Score  = " + totalScore);

    }
}
