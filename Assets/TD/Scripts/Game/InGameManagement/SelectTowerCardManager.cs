using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTowerCardManager : MonoBehaviour
{
    [Header("Card Elements")]
    [SerializeField] private GameObject CardPanel;
    [SerializeField] private Image towerIcon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI towerNameText;

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
    private UpgradeDefinition nextUpgradeA;
    private UpgradeDefinition nextUpgradeB;
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

    private void Update()
    {
        if (CardPanel.activeSelf && towerSelected != null)
        {
            UpdatePurchaseButtonsInteractability();
        }
    }

    public void ShowSelectCard(Tower tower)
    {
        towerSelected = tower;

        towerIcon.sprite = towerSelected.towerData.TowerSprite;
        towerNameText.text = towerSelected.towerData.towerName;

        PriceToSell = towerSelected.SellValue;
        priceText.text = "" + PriceToSell;

        RefreshUI(towerSelected);

        CardPanel.SetActive(true);
    }
    public void HideSelectCard()
    {
        towerSelected = null;
        nextUpgradeA = null;
        nextUpgradeB = null;
        CardPanel.SetActive(false);
    }

    public void RefreshUI(Tower tower)
    {
        if (tower != towerSelected) return;

        PriceToSell = towerSelected.SellValue;
        priceText.text = "" + PriceToSell;

        nextUpgradeA = tower.GetNextUpgrade(0);
        nextUpgradeB = tower.GetNextUpgrade(1);


        UpdateUpgradeButtonUI(pathA_Button, pathA_Name, pathA_Desc, pathA_Cost, /*pathA_LockedIcon,*/ tower, 0);

        UpdateUpgradeButtonUI(pathB_Button, pathB_Name, pathB_Desc, pathB_Cost, /*pathB_LockedIcon,*/ tower, 1);
        
        UpdatePurchaseButtonsInteractability();
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
            button.interactable = true;
        }
    }

    private void UpdatePurchaseButtonsInteractability()
    {
        if (nextUpgradeA != null && !towerSelected.IsPathLocked(0))
        {
            pathA_Button.interactable = PlayerEconomy.Instance.CanBuy(nextUpgradeA.upgradeCost);
        }
        
        if (nextUpgradeB != null && !towerSelected.IsPathLocked(1))
        {
            pathB_Button.interactable = PlayerEconomy.Instance.CanBuy(nextUpgradeB.upgradeCost);
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
