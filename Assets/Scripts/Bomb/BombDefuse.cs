using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BombDefuse : MonoBehaviour
{
    [SerializeField] private string inputText;

    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;
    public string correctPhoneNumber = "5551234";  // The correct phone number to defuse the bomb
    public GameObject bombUI;  // The bomb UI to hide after defuse
    public GameObject explosionUI; // UI to show on failure (optional)

    // Call this function when the player submits the phone number
    /*
    public void TryDefuse()
    {
        if (phoneNumberInput.text == correctPhoneNumber)
        {
            DefuseBomb();
        }
        else
        {
            ExplodeBomb();
        }
    }

    void DefuseBomb()
    {
        // Hide the bomb UI
        bombUI.SetActive(false);
        Debug.Log("Bomb defused!");
        // You can add more logic here (like sound effects, animations, etc.)
    }

    void ExplodeBomb()
    {
        // Optionally show explosion UI or effects
        bombUI.SetActive(false);
        if (explosionUI != null)
        {
            explosionUI.SetActive(true);
        }
        Debug.Log("Bomb exploded!");
    }
    */
}
