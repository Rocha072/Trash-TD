using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    public int totalTrashColleted { get; private set; }
    public int totalTowerPurchased { get; private set; }
    public int totalPollution { get; private set; } 
    public int moneySpent { get; private set; }
    public int moneyGeneratedByCollections { get; private set; } 
    public int moneyGeneratedByWaves { get; private set; } 
    public int totalMoneyAccumulated { get { return moneyGeneratedByCollections + moneyGeneratedByWaves; }}

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

        totalTrashColleted = 0;
        totalTowerPurchased = 0;
        totalPollution = 0;
        moneySpent = 0;
        moneyGeneratedByCollections = 0;
        moneyGeneratedByWaves = 0;
    }


    public void AddTrashCollected()
    {
        totalTrashColleted++;
    }

    public void AddTowerPurchased()
    {
        totalTowerPurchased++;
    }

    public void AddPolution(int amount)
    {
        totalPollution += amount;
    }

    public void AddMoneySpent(int amount)
    {
        moneySpent += amount;
    }

    public void AddMoneyGeneratedByCollections(int amount)
    {
        moneyGeneratedByCollections += amount;
    }

    public void AddMoneyGeneratedByWaves(int amount)
    {
        moneyGeneratedByWaves += amount;
    }
}
