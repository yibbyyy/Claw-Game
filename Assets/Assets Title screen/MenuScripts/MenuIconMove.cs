using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    public RectTransform icon;  
    public Button[] menuButtons;
    public float moveSpeed = 10f;  
    public Vector2 offset = new Vector2(-50, 0);  

    private RectTransform currentButton;  

    void Start()
    {
        
        currentButton = menuButtons[0].GetComponent<RectTransform>();
        icon.position = currentButton.position + (Vector3)offset;

       
        foreach (Button button in menuButtons)
        {
            button.onClick.AddListener(() => OnButtonSelected(button));
        }
    }

    void Update()
    {
        
        Vector3 targetPosition = currentButton.position + (Vector3)offset;
        if (icon.position != targetPosition)
        {
            icon.position = Vector3.Lerp(icon.position, targetPosition, Time.unscaledDeltaTime * moveSpeed);
        }
    }
    
    void OnButtonSelected(Button selectedButton)
    {
        
        currentButton = selectedButton.GetComponent<RectTransform>();
    }

    
    public void MoveIconToButton(Button button)
    {
        currentButton = button.GetComponent<RectTransform>();
    }
}
