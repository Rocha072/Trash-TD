using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    public static PlayerEconomy Instance { get; private set; }
    [SerializeField] private int money = 200;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void GainMoney(int amount)
    {
        money += amount;
    }
    
    public int GetMoney()
    {
        return money;
    }

    public bool CanBuy(Tower tower)
    {
        if (money >= tower.towerData.cost)
        {
            return true;
        }
        return false;
    }

    public bool CanBuy(int ID)
    {
        if (money >= EntitySummoner.Instance.towerBlueprints[ID].towerData.cost)
        {
            return true;
        }
        return false;
    }

    public void Buy(Tower tower)
    {
        money -= tower.towerData.cost;
    }


}