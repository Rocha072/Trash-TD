using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    [SerializeField] private int money = 200;

    public static PlayerEconomy Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GainMoney(int amount)
    {
        money += amount;
        UIManager.Instance.UpdateMoneyText();
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
        UIManager.Instance.UpdateMoneyText();
    }


}