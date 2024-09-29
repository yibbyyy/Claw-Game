using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  
    private bool isPaused = false;  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

   
    public void Resume()
    {
        pauseMenuUI.SetActive(false);     
        isPaused = false;
    }

   
    void Pause()
    {
        pauseMenuUI.SetActive(true);       
        isPaused = true;
    }

    
    public void RestartGame()
    {
        Time.timeScale = 1f;  // game speed 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // This should restart the active scene
    }

    public void LoadMainMenu() //Go back to the main menu
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("MainMenu"); 
    }

    
    public void QuitGame()
    {
        Application.Quit();   
        Debug.Log("Quitting game...");  
    }
}
