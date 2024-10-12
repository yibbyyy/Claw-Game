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
        SceneManager.LoadScene("Loading");
        
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

    public void ReturnButton()
    {
        
        if (GameManager.instance.isPausedFromGame)
        {
            SceneManager.LoadScene("JasperScene");  //Insert Game Scene name!!!
            GameManager.instance.SetPausedFromGame(false);  
        }
        else
        {
            SceneManager.LoadScene("AlienGame");  
        }
    }

}

