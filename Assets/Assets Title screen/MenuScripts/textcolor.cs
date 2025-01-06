using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonTextColorChangerTMP : MonoBehaviour
{
    public TextMeshProUGUI buttonText; 
    public Color normalColor = Color.white;
    public Color highlightedColor = Color.yellow;
    public Color pressedColor = Color.red;

    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnPressed);
    }

    private void OnEnable()
    {
        buttonText.color = normalColor;
    }

    void OnPressed()
    {
        buttonText.color = pressedColor;
    }

    void OnHighlighted()
    {
        buttonText.color = highlightedColor;
    }

    void OnNormal()
    {
        buttonText.color = normalColor;
    }
}
