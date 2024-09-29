using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class MainMenuController : MonoBehaviour
{
    
    public void tutorial()
    {
        SceneManager.LoadScene("Tutorial1"); 
    }

     public void Next()
    {
        SceneManager.LoadScene("TutorialScene2"); 
    }

    public void play()
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
