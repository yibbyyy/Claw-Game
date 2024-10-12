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
                StartCoroutine(DiageticClick());
            }
        } 
        
    }

    public void ClickButton()
    {

    }

    IEnumerator DiageticClick()
    {
        click?.Invoke();
        clickable = false;
        gameObject.transform.Translate(Vector3.down * .03125f);
        yield return new WaitForSeconds(.5f);
        gameObject.transform.Translate(Vector3.up * .03125f);
        
    }

}
