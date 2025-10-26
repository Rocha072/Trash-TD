using UnityEngine;
using UnityEngine.UI;

public class InfoButtonManagement : MonoBehaviour
{
    [Header("Referências")]
    public RectTransform shopRect; 

    [Header("Configurações de Posição")]
    public float padding = 20f;

    private RectTransform infoButtonRect; 
    private float defaultPosX;      
    private float defaultPosY;    

    void Start()
    {
        infoButtonRect = GetComponent<RectTransform>();
     
        defaultPosX = infoButtonRect.anchoredPosition.x;
        defaultPosY = infoButtonRect.anchoredPosition.y;
    }

    void LateUpdate()
    {
   
        if (shopRect != null && shopRect.gameObject.activeInHierarchy)
        {
            
            float sideBarAnchorX = shopRect.anchoredPosition.x;
            
            
            float sideBarWidth = shopRect.rect.width;

            float sideBarLeftEdgeX = sideBarAnchorX - sideBarWidth;


            infoButtonRect.anchoredPosition = new Vector2(sideBarLeftEdgeX - padding, defaultPosY);
        }
        else
        {
  
            infoButtonRect.anchoredPosition = new Vector2(defaultPosX, defaultPosY);
        }
    }
}
