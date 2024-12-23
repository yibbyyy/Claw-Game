using System.Collections;
using System;
using UnityEditor;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public bool clickable = false;
    public static event Action click;
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (clickable && hit.collider == gameObject.GetComponent<Collider>() && Input.GetMouseButtonDown(0))
            {
                // May start coroutine multiple times before clickable is set to false
                Debug.Log("If statement of clicked");
                clickable = false;
                click?.Invoke();
                StartCoroutine(DiageticClick());
            }
        } 
        
    }

    public void GameEnded()
    {
        clickable = false;
    }

    public void GameStarted()
    {
        clickable = true;
    }

    IEnumerator DiageticClick()
    {
        
        // TODO potentially set clickable to equal false before coroutine is called to avoid race conditions with claw manager
       
        gameObject.transform.Translate(Vector3.down * .03125f);
        yield return new WaitForSeconds(.5f);
        gameObject.transform.Translate(Vector3.up * .03125f);
        
    }

}
