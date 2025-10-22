using UnityEngine;

public class Shop : MonoBehaviour
{
    public MousePosition mouse;
    public void PurchaseTower(int towerID)
    {
        mouse.setTowerToPlaceByID(0);
    }
}
