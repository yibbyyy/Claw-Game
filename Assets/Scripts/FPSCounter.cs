using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsText;  // Assign the Text component in the Inspector
    private float deltaTime = 0.0f;
    private float fpsUpdateInterval = 0.5f;  // Time interval to update the FPS value
    private float timeLeft;  // Time left to next FPS update

    void Start()
    {
        timeLeft = fpsUpdateInterval;
    }

    void Update()
    {
        // Add the unscaled delta time for this frame (how long the frame took)
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Decrease time left until the next FPS update
        timeLeft -= Time.deltaTime;

        // If the interval time has passed, update the displayed FPS
        if (timeLeft <= 0.0)
        {
            // Calculate FPS based on delta time
            float fps = 1.0f / deltaTime;

            // Update the UI text with the average FPS (rounded to whole number)
            fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";

            // Reset time left to the next update
            timeLeft = fpsUpdateInterval;
        }
    }
}
