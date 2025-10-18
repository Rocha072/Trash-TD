using UnityEngine;

public class Shop : MonoBehaviour
{
    public MousePosition mouse;
    public void PurchaseWaterGun()
    {
        mouse.setTowerToPlaceByID(0);
    }
}
