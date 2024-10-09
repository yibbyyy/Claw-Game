using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public bool clickable = false;
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
        clickable = false;
        gameObject.transform.Translate(Vector3.down * .03125f);
        yield return new WaitForSeconds(.5f);
        gameObject.transform.Translate(Vector3.up * .03125f);
        clickable = true;
    }

}
