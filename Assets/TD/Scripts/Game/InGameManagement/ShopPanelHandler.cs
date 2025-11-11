using UnityEngine;
using UnityEngine.EventSystems;
public class ShopPanelHandler : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        
        TowerData pendingTower = UIManager.Instance.pendingTowerToSpawn;

        if (pendingTower != null && PlayerEconomy.Instance.CanBuy(pendingTower.cost))
        {
            int towerID = pendingTower.towerID;
            UIManager.Instance.ClearPendingTower();
            MousePosition.Instance.setTowerToPlaceByID(towerID);
        }
    }
}

