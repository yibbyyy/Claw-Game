using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class MainMenuController : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    
    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu"); 
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditsMenu"); 
    }

    public void QuitGame()
    {
        Application.Quit(); 
    
    }
    
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

}
