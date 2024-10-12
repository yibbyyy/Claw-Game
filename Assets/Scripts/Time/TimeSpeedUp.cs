using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeedUp : MonoBehaviour
{
    void Update()
    {
        // Check if the player presses the 'F' key to fast-forward
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Double the speed of time
            Time.timeScale = 2f;
        }

        // Check if the player presses the 'N' key to return to normal speed
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Reset time scale back to normal
            Time.timeScale = 1f;
        }
    }

}
