using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPausedFromGame = false;  

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void SetPausedFromGame(bool paused)
    {
        isPausedFromGame = paused;
    }
}
