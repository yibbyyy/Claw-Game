using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MenuSelector menuSelector;

    public void OnPointerEnter(PointerEventData eventData)
    {
      
        menuSelector.MoveIconToButton(GetComponent<Button>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
