using System;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool paused = false;
    public GameObject pauseUI;

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
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            return false;
        }
        else
        {
            pauseUI.SetActive(true);    
            Time.timeScale = 0f;
            return true;
        }
    }

}
