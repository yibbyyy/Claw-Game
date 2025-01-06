using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public Pause pause;
    public GameObject optionsUI;


    public void ResumeGame()
    {
        pause.paused = pause.togglePause();
    }


    public void RestartGame()
    {
        pause.paused = pause.togglePause();
        SceneManager.LoadSceneAsync("Loading");
    }


    public void OpenOptions()
    {
        gameObject.SetActive(false);
        optionsUI.SetActive(true);
    }


    public void ReturnToTitle()
    {
        pause.paused = pause.togglePause();
        SceneManager.LoadScene("MainMenu");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
