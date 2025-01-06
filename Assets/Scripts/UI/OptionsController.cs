using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    public GameObject pauseUi;


    public void Back()
    {
        gameObject.SetActive(false);
        pauseUi.SetActive(true);
    }


}
