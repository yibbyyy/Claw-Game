using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public string sceneToLoad = "JasperScene"; // The name of the game scene to load

    void Start()
    {
        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        Debug.Log("loading scene");
        // Optionally, you can display a loading progress
        while (!asyncLoad.isDone)
        {
            Debug.Log("done!");
            // You can update a loading bar or display progress here
            yield return null; // Wait until the next frame
        }
    }
}
