using UnityEngine;
using UnityEngine.EventSystems; 


[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ButtonHoverCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetHand();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefault();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefault();

    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefault();
    }
}