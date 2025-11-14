using UnityEngine;
using UnityEngine.EventSystems; 


[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ButtonHoverCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private bool isScreenChanger = true;
   

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetHand();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefault();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isScreenChanger)
        {
            CursorManager.Instance.SetDefault();
        }
    }
}