using UnityEngine;
using UnityEngine.EventSystems;
public class ShopPanelHandler : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {

        TowerData pendingTower = UIManager.Instance.pendingTowerToSpawn;
        if (pendingTower == null || !PlayerEconomy.Instance.CanBuy(pendingTower.cost))
            return;

        if (IsMouseInsideGameWindow())
        {
            int towerID = pendingTower.towerID;
            MousePosition.Instance.setTowerToPlaceByID(towerID);
        }
    }
    
    private bool IsMouseInsideGameWindow()
    {
        Vector3 mousePos = Input.mousePosition;
        
        
        if (mousePos.x < 0 || mousePos.x > Screen.width ||
            mousePos.y < 0 || mousePos.y > Screen.height)
        {
            return false; 
        }
        
        return true; 
    }
}

