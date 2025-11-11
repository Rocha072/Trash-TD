using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeScreenManager : MonoBehaviour
{
    [Header("Tower Base")]
    [SerializeField] private Image towerIcon;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerDescriptionText;
    [SerializeField] private TextMeshProUGUI towerCost;

    // [Header("Stats Base")]
    // [SerializeField] private TextMeshProUGUI baseCostText;
    // [SerializeField] private TextMeshProUGUI baseDamageText;
    // [SerializeField] private TextMeshProUGUI baseRangeText;
    // [SerializeField] private TextMeshProUGUI baseFireRateText;

    [Header("Path A")]
    [SerializeField] private TextMeshProUGUI pathA_Name_1;
    [SerializeField] private TextMeshProUGUI pathA_Desc_1;
    [SerializeField] private TextMeshProUGUI pathA_Cost_1;
    [SerializeField] private TextMeshProUGUI pathA_Name_2;
    [SerializeField] private TextMeshProUGUI pathA_Desc_2;
    [SerializeField] private TextMeshProUGUI pathA_Cost_2;
    [SerializeField] private TextMeshProUGUI pathA_Name_3;
    [SerializeField] private TextMeshProUGUI pathA_Desc_3;
    [SerializeField] private TextMeshProUGUI pathA_Cost_3;

    [Header("Path B")]
    [SerializeField] private TextMeshProUGUI pathB_Name_1;
    [SerializeField] private TextMeshProUGUI pathB_Desc_1;
    [SerializeField] private TextMeshProUGUI pathB_Cost_1;
    [SerializeField] private TextMeshProUGUI pathB_Name_2;
    [SerializeField] private TextMeshProUGUI pathB_Desc_2;
    [SerializeField] private TextMeshProUGUI pathB_Cost_2;
    [SerializeField] private TextMeshProUGUI pathB_Name_3;
    [SerializeField] private TextMeshProUGUI pathB_Desc_3;
    [SerializeField] private TextMeshProUGUI pathB_Cost_3;
    
    public static UpgradeScreenManager Instance { get; private set; }

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

    public void UpdateInfo(TowerData data)
    {
        towerIcon.sprite = data.TowerSprite;
        towerNameText.text = data.towerName;
        towerDescriptionText.text = data.towerDescription;
        towerCost.text = data.cost.ToString();
        // baseCostText.text = "Custo: " + data.cost;
        // baseDamageText.text = "Dano: " + data.baseDamage;
        // baseRangeText.text = "Alcance: " + data.baseRange;
        // baseFireRateText.text = "Cadência: " + data.baseFireRate;

        FillUpgradeSlot(data.pathA_Upgrades, 0, pathA_Name_1, pathA_Desc_1, pathA_Cost_1);
        FillUpgradeSlot(data.pathA_Upgrades, 1, pathA_Name_2, pathA_Desc_2, pathA_Cost_2);
        FillUpgradeSlot(data.pathA_Upgrades, 2, pathA_Name_3, pathA_Desc_3, pathA_Cost_3);
        
    
        FillUpgradeSlot(data.pathB_Upgrades, 0, pathB_Name_1, pathB_Desc_1, pathB_Cost_1);
        FillUpgradeSlot(data.pathB_Upgrades, 1, pathB_Name_2, pathB_Desc_2, pathB_Cost_2);
        FillUpgradeSlot(data.pathB_Upgrades, 2, pathB_Name_3, pathB_Desc_3, pathB_Cost_3);
    }

    private void FillUpgradeSlot(UpgradeDefinition[] path, int index, TextMeshProUGUI name, TextMeshProUGUI desc, TextMeshProUGUI cost)
    {
        
        if (index < path.Length && path[index] != null)
        {
            UpgradeDefinition upgrade = path[index];
            name.text = upgrade.upgradeName;
            desc.text = upgrade.upgradeDescription;
            cost.text = upgrade.upgradeCost.ToString();
        }
        else
        {
            name.text = "---";
            desc.text = "---";
            cost.text = "---";
        }
    }
}
