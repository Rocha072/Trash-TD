using UnityEngine;
using TMPro;

public class VictoryScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalTrashCollectedText;
    [SerializeField] private TextMeshProUGUI totalTowerPurchasedText;
    [SerializeField] private TextMeshProUGUI totalPollutionText;
    [SerializeField] private TextMeshProUGUI moneySpentText;
    [SerializeField] private TextMeshProUGUI moneyGeneratedByColletionsText;
    [SerializeField] private TextMeshProUGUI totalMoneyAccumulatedText;

    public void showStats()
    {
        StatsManager stats = StatsManager.Instance;

        totalTrashCollectedText.text = stats.totalTrashColleted.ToString();
        totalTowerPurchasedText.text = stats.totalTowerPurchased.ToString();
        totalPollutionText.text = stats.totalPollution.ToString();
        moneySpentText.text = stats.moneySpent.ToString();
        moneyGeneratedByColletionsText.text = stats.moneyGeneratedByCollections.ToString();
        totalMoneyAccumulatedText.text = stats.totalMoneyAccumulated.ToString();
    }
}
