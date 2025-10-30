using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTowerCardManager : MonoBehaviour
{
    [Header("Card Elements")]
    [SerializeField] private GameObject CardPanel;
    [SerializeField] private Image towerIcon;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Upgrade Path A Elements")]
    [SerializeField] private Button pathA_Button;
    [SerializeField] private TextMeshProUGUI pathA_Name;
    [SerializeField] private TextMeshProUGUI pathA_Desc;
    [SerializeField] private TextMeshProUGUI pathA_Cost;

    //[SerializeField] private GameObject pathA_LockedIcon;

    [Header("Upgrade Path B Elements")]
    [SerializeField] private Button pathB_Button;
    [SerializeField] private TextMeshProUGUI pathB_Name;
    [SerializeField] private TextMeshProUGUI pathB_Desc;
    [SerializeField] private TextMeshProUGUI pathB_Cost;

    //[SerializeField] private GameObject pathB_LockedIcon;
    
    public static SelectedTowerCardManager Instance { get; private set; }

    private Tower towerSelected;

    private int PriceToSell;

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
  
    public void ShowSelectCard(Tower tower)
    {
        towerSelected = tower;
        towerIcon.sprite = towerSelected.towerData.TowerSprite;

        //chamar funcao para calculo atual de custo
        PriceToSell = Mathf.RoundToInt(towerSelected.towerData.cost * 0.7f);
        priceText.text = "" + PriceToSell;

        RefreshUI(tower);

        CardPanel.SetActive(true);
    }
    public void HideSelectCard()
    {
        towerSelected = null;
        CardPanel.SetActive(false);
    }

    public void RefreshUI(Tower tower)
    {
        if (tower != towerSelected) return;

        UpdateUpgradeButtonUI(pathA_Button, pathA_Name, pathA_Desc, pathA_Cost, /*pathA_LockedIcon,*/ tower, 0);
        
        UpdateUpgradeButtonUI(pathB_Button, pathB_Name, pathB_Desc, pathB_Cost, /*pathB_LockedIcon,*/ tower, 1);
    }

    private void UpdateUpgradeButtonUI(Button button, TextMeshProUGUI nameText, TextMeshProUGUI descText, TextMeshProUGUI costText, Tower tower, int pathIndex)
    {
        UpgradeDefinition nextUpgrade = tower.GetNextUpgrade(pathIndex);

        if (tower.IsPathLocked(pathIndex))
        {
            button.interactable = false;
            nameText.text = "Bloqueado";
            costText.text = "---";
            descText.text = "---";
            //icon bloqueado
        }
        else if (nextUpgrade == null)
        {
            button.interactable = false;
            nameText.text = "Nível Máximo";
            costText.text = "---";
            descText.text = "---";
            //icon verde
        }
        else
        {
            //desativar icon

            nameText.text = nextUpgrade.upgradeName;
            costText.text = "" + nextUpgrade.upgradeCost;
            descText.text = "" + nextUpgrade.upgradeDescription;
            // Desativa o botão se o jogador não tiver dinheiro
            //button.interactable = PlayerEconomy.Instance.CanBuy(nextUpgrade.cost);
        }
    }

   
    public void OnPathClicked(int pathIndex)
    {
        if (towerSelected != null)
        {
            towerSelected.TryApplyUpgrade(pathIndex);
        }
    }

   

    public void SellTower()
    {
        Tower towerToRemove = towerSelected;
        PlayerEconomy.Instance.GainMoney(PriceToSell);
        MousePosition.Instance.DeselectTower();
        EntitySummoner.Instance.RemoveTower(towerToRemove);

    }
    


}
