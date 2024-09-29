using UnityEngine;

public class BombActivator : MonoBehaviour
{
    public GameObject bombUI;  // Reference to the bomb UI Canvas

    // This function will be called by the button
    public void ActivateBomb()
    {
        bombUI.SetActive(true);  // Show the bomb UI
        Debug.Log("Bomb UI Activated!" + bombUI.activeSelf);
    }
}
