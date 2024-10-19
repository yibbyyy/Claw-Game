using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private string highScoreKey = "HighScore";

    public void SaveHighScore( int score)
    {
        int currentHighScore = PlayerPrefs.GetInt(highScoreKey, 0);

        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save(); // write to disk
            Debug.Log("New High Score:" + score);
        }
    }

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(highScoreKey, 0);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(highScoreKey); // deletes score
        Debug.Log("High Score Reset");
    }
}
