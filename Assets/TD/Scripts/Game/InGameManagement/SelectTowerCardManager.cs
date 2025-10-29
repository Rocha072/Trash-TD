using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTowerCardManager : MonoBehaviour
{
    [SerializeField] private GameObject CardPanel;
    [SerializeField] private Image towerIcon;
    [SerializeField] private TextMeshProUGUI priceText;

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
        PriceToSell = Mathf.RoundToInt(towerSelected.towerData.cost * 0.7f);
        priceText.text = "" + PriceToSell;
        CardPanel.SetActive(true);
    }
    public void HideSelectCard()
    {
        towerSelected = null;
        CardPanel.SetActive(false);
    }

    public void SellTower()
    {
        Tower towerToRemove = towerSelected;
        PlayerEconomy.Instance.GainMoney(PriceToSell);
        MousePosition.Instance.DeselectTower();
        EntitySummoner.Instance.RemoveTower(towerToRemove);
        
    }

}
