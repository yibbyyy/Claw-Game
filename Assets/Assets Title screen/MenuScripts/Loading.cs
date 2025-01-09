using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public string sceneToLoad; // The name of the game scene to load
    public float loadTime;

    void Start()
    {
        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Start loading the scene
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadSceneAsync(sceneToLoad);
        Debug.Log("loading scene");



    }
}
