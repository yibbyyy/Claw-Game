using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PushandPull : GenericBomb
{
    
    public GameObject SucessText, pushButton, Pin;
    public float startDelay = 1f;
    public float timerDuration = 5f;
    public float disposeDelay = .5f;

    // For pin
    public float ogX = 220, pinY = -29, pulledX = 360;

    
   
    private AudioSource audioSource;
    public bool win = false;
    



    
    public bool buttonPressed = false, pinPulled = false;
    private void OnEnable()
    {
       

       // General Setup
        win = false;
        bombTimerUI.text = bombTimerDuration.ToString("F2");
        audioSource = GetComponent<AudioSource>();
        Pin.GetComponent<RectTransform>().anchoredPosition = new Vector2(ogX, pinY);

        // User Setup
        StartCoroutine(Timer(timerDuration));
        ToggleInteractibility(true);
        ToggleButtonSubscription(true);

        buttonPressed = false; pinPulled = false;

        




    }

    
   

    


    
        
    

    
    public void ClickedButton()
    {
        //Debug.Log("Clicked Button was called by: " + EventSystem.current.currentSelectedGameObject.name);
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null)
        {
            // Remove interactibility and listener for interactible
            clickedButton.GetComponent<Button>().interactable = false;
            clickedButton.GetComponent<Button>().onClick.RemoveListener(ClickedButton);

            if (!buttonPressed)
            {
                if (clickedButton == pushButton)
                {
                    buttonPressed = true;
                }

                else
                {
                    if (!exploded)
                    {
                        //Debug.Log("Bomb blows up");
                        StartCoroutine(BombExplosion());
                    }
                }
                

            }
            else if (!pinPulled)
            {
                if (clickedButton == Pin)
                {
                    Pin.GetComponent<RectTransform>().anchoredPosition = new Vector2(pulledX, pinY);
                    // Defuse successful!
                    StopAllCoroutines();
                    // Can fire event and/or just change exploded to true so timer doesn't go off
                    win = true;
                    exploded = true;
                    Dispose();
                }
                else
                {
                    if (!exploded)
                    {
                        //Debug.Log("Bomb blows up");
                        StartCoroutine(BombExplosion());
                    }
                }
            }

            
           
            
        }
    }

    
    
    private void ToggleButtonSubscription(bool toggle)
    {
        if (toggle)
        {
            // Subscribe ClickedButton() to all button onclick events
            
            
            pushButton.GetComponent<Button>().onClick.AddListener(ClickedButton);
            Pin.GetComponent<Button>().onClick.AddListener(ClickedButton);

        }

        else
        {
            pushButton.GetComponent<Button>().onClick.RemoveListener(ClickedButton);
            Pin.GetComponent<Button>().onClick.RemoveListener(ClickedButton);
        }
    }
    private void ToggleInteractibility(bool toggle)
    {
        pushButton.GetComponent<Button>().interactable = toggle;
        Pin.GetComponent<Button>().interactable = toggle;

    }

   
        
    
    IEnumerator DisposeDelay()
    {
        float duration = 0f;

        while (duration < disposeDelay)
        {
            duration += Time.deltaTime;
            yield return null;
        }

        if (win)
        {
            audioSource.Play();
            SucessText.SetActive(true);
            duration = 0f;

            while (duration < 2.5f)
            {
                duration += Time.deltaTime;
                yield return null;
            }
            SucessText.SetActive(false);
        }
       
        
        
        
        exploded = false;
        base.Dispose();
    }

    // Call dispose from othe r file
    public override void Dispose()
    {
        
        bombTimerUI.text = bombTimerDuration.ToString("F2");
        ToggleInteractibility(false);
        ToggleButtonSubscription(false);
        
        StartCoroutine(DisposeDelay());

    }
}
