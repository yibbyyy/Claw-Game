using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool paused = false;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            paused = togglePause();

    }

    private void OnGUI()
    {
        if (paused)
        {
            GUILayout.Label("Game is paused!");
            if (GUILayout.Button("Resume"))
                paused = togglePause();
        }
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return false;
        }
        else
        {
            Time.timeScale = 0f;
            return true;
        }
    }

}
