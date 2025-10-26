using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [Header("Lifes/Money")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI lifeText;

    [Header("Mouse and guide")]
    public MousePosition mouse;
    public GameObject guidePanel;


    public static UIManager Instance { get; private set; }

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (guidePanel.activeSelf)
                hideGuidePanel();
            else
                showGuidePanel();
        }
    }
    
    public void UpdateMoneyText()
    {
        moneyText.text = "" + PlayerEconomy.Instance.GetMoney().ToString();
    }

    public void UpdateLifeText()
    {
        lifeText.text = "" + PlayerLife.Instance.GetLife().ToString();
    }


    public void PurchaseTower(int towerID)
    {
        if (PlayerEconomy.Instance.CanBuy(towerID))
        {
            mouse.setTowerToPlaceByID(towerID);
        }
    }

    public void showGuidePanel()
    {
        guidePanel.SetActive(true);
    }
    
    public void hideGuidePanel()
    {
        guidePanel.SetActive(false);
    }
}