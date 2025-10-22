using UnityEngine;

public class Shop : MonoBehaviour
{
    public MousePosition mouse;
    public void PurchaseTower(int towerID)
    {
        if (PlayerEconomy.Instance.CanBuy(towerID))
        {
            mouse.setTowerToPlaceByID(towerID);
        }
    }
}
