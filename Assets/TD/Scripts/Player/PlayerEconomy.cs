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

    public bool CanBuy(int cost)
    {
        return money >= cost;
    }

    public void Buy(int cost)
    {
        money -= cost;
        UIManager.Instance.UpdateMoneyText();
    }


}