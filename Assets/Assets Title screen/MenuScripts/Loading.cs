using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public string sceneToLoad; // The name of the game scene to load
    public float loadTime;

    void Awake()
    {

        Debug.Log("Gothere");
        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        Debug.Log("loading scene");

        // Start loading the scene
        yield return new WaitForSeconds(loadTime);
        Debug.Log("GotThroughWaitLoading");
        SceneManager.LoadSceneAsync(sceneToLoad);
        
        StopCoroutine(LoadSceneAsync());
    }
}
