using UnityEngine;
using UnityEngine.EventSystems; 


[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ButtonHoverCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private bool isScreenChanger = true;
    [SerializeField] private AudioClip clickAudio;

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

        if (clickAudio != null)
        {
            SoundHandler.Instance.PlayUISound(clickAudio, 0.9f);
        }
    }
}