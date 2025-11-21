using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI currentWaveText;

    [Header("Mouse and Pause Screen")]
    public MousePosition mouse;
    public GameObject pauseScreen;
    public GameObject gameGuide;
    public GameObject optionsScreen;

    [Header("Shop Bar")]
    public GameObject Shop;
    public CanvasGroup ShopCanvasGroup;
    public GameObject purchaseTowerButtonPrefab;
    private class ShopButtonData
    {
        public TowerData towerData;
        public Button buttonComponent;
        public Image buttonBackground;
         public TextMeshProUGUI towerName;
        public Image towerIcon;
        public TextMeshProUGUI towerPrice;
        public Image coin;
        public ButtonHoverCursor hoverDetector;
    }
    private List<ShopButtonData> shopButtons = new List<ShopButtonData>();
    private Color color_CanBuy_Background = new Color(0.2005482f, 0.7149413f, 0.8641509f, 0.4823529f);
    private Color color_CantBuy_Background = new Color(0.1226415f, 0.4078374f, 0.490566f, 0.4823529f);
    private Color color_Selected_Background = new Color(0.08116768f, 0.2525773f, 0.3018868f, 0.4823529f);
    private Color color_CanBuy = Color.white;
    private Color color_CantBuy = new Color(0.85f, 0.85f, 0.85f, 1f); 
    private Color color_Selected = new Color(0.5f, 0.5f, 0.5f, 1f); 

    public TowerData pendingTowerToSpawn { get; private set; } = null;

    [Header("Victory/Defeat Screen")]
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private VictoryScreenUI VictoryScreenScript;
    [SerializeField] private GameObject DefeatScreen;
    [SerializeField] private TextMeshProUGUI completedWavesText;

    [Header("Upgrade Screen")]
    [SerializeField] private GameObject upgradeScreen;
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

    void Start()
    {
        Time.timeScale = 1f;
        SetShop();
        InvokeRepeating(nameof(UpdateShopButtonColors), 0f, 0.2f);
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeSelf)
            {
                hidePauseScreen();
            }
            else
            {
                showPauseScreen();
            }

        }

        if (!pauseScreen.activeSelf && Input.GetKeyDown(KeyCode.Tab))
        {
            if (Shop.activeSelf)
            {
                CloseShopAndClearPending();
            }
            else
            {
                PlayerMovement.TurnOffPlayerMovement();
                Shop.SetActive(true);
                SelectedTowerCardManager.Instance.HideSelectCard();
                ClearPendingTower();
            }

        }
    }

    public void ClearPendingTower()
    {
        pendingTowerToSpawn = null;
        UpdateShopButtonColors();
    }
    
    public void CloseShopAndClearPending()
    {
        PlayerMovement.TurnOnPlayerMovement();
        Shop.SetActive(false);
        ClearPendingTower();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "" + PlayerEconomy.Instance.GetMoney().ToString();
    }

    public void UpdateLifeText()
    {
        lifeText.text = (100 - PlayerLife.Instance.GetLife()).ToString() + "%";
    }

    public void UpdateCurrentWaveText()
    {
        currentWaveText.text = "Rodada " + (WaveManager.Instance.currentWaveIndex+1) + "/" + WaveManager.Instance.maxWavesForThisDifficulty;
    }

    public void SelectTowerInShop(TowerData data)
    {
        if (pendingTowerToSpawn == data)
        {
            pendingTowerToSpawn = null;
        }
        else
        {
            pendingTowerToSpawn = data;
        }

        UpdateShopButtonColors();
    }

    public void OnUpgradeScreenButtonClicked()
    {
        TowerData dataToShow = null;

        Tower selectedTower = SelectedTowerCardManager.Instance.GetSelectedTower();
        if (selectedTower != null)
        {
            dataToShow = selectedTower.towerData;
        }

        else if (pendingTowerToSpawn != null)
        {
            dataToShow = pendingTowerToSpawn;
        }

        if (dataToShow != null)
        {
            ShowUpgradeScreen(dataToShow);
        }

        ClearPendingTower();
    }

    private void ShowUpgradeScreen(TowerData data)
    {
        Time.timeScale = 0f;
        UpgradeScreenManager.Instance.UpdateInfo(data);
        upgradeScreen.SetActive(true);
    }
    
    public void HideUpgradeScreen()
    {
        upgradeScreen.SetActive(false);
        if (!pauseScreen.activeSelf)
        {
            Time.timeScale = 1.0f;
        }
    }

    public void showPauseScreen()
    {
        Time.timeScale = 0f;
        PlayerMovement.TurnOffPlayerMovement();
        pauseScreen.SetActive(true);
        optionsScreen.SetActive(true);
        gameGuide.SetActive(false);

        if (Shop.activeSelf)
            ShopCanvasGroup.interactable = false;
    }

    public void hidePauseScreen()
    {
        Time.timeScale = 1.0f;
        pauseScreen.SetActive(false);
        optionsScreen.SetActive(false);
        gameGuide.SetActive(false);

        if (!Shop.activeSelf)
            PlayerMovement.TurnOnPlayerMovement();
        else
            ShopCanvasGroup.interactable = true;
    }


    public void showGameGuide()
    {
        optionsScreen.SetActive(false);
        gameGuide.SetActive(true);
    }

    public void hideGameGuide()
    {
        optionsScreen.SetActive(true);
        gameGuide.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void showVictoryScreen()
    {
        VictoryScreenScript.showStats();
        VictoryScreen.SetActive(true);
        Time.timeScale = 0f;
        PlayerMovement.TurnOffPlayerMovement();
    }

    public void ShowDefeatScreen()
    {
        DefeatScreen.SetActive(true);
        completedWavesText.text = "Rodada " + WaveManager.Instance.currentWaveIndex + "/" + WaveManager.Instance.allWaves.Count;
        Time.timeScale = 0f;
        PlayerMovement.TurnOffPlayerMovement();
    }

    public void SetShop()
    {
        foreach (TowerBlueprint towerBlueprint in EntitySummoner.Instance.towerBlueprints)
        {
            TowerData data = towerBlueprint.towerData;

            GameObject newButton = Instantiate(purchaseTowerButtonPrefab);

            newButton.transform.SetParent(Shop.transform, false);

            TextMeshProUGUI towerName = newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            towerName.text = data.towerName;
            
            Image towerImage = newButton.transform.GetChild(1).GetComponent<Image>();
            towerImage.sprite = data.TowerSprite;
            
            TextMeshProUGUI towerPrice = newButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            towerPrice.text = data.cost.ToString();

            
            Button buyTowerButton = newButton.GetComponent<Button>();
            buyTowerButton.onClick.RemoveAllListeners();
            buyTowerButton.onClick.AddListener(() => SelectTowerInShop(data));


            Image buttonBackground = newButton.GetComponent<Image>();
            Image coinImage = newButton.transform.GetChild(3).GetComponent<Image>();

            shopButtons.Add(new ShopButtonData
            {
                towerData = data,
                buttonComponent = buyTowerButton,
                buttonBackground = buttonBackground,
                towerName = towerName,
                towerIcon = towerImage,
                towerPrice = towerPrice,
                coin = coinImage
            });
        }
    }


    void UpdateShopButtonColors()
    {
        if (!Shop.activeSelf) return;

        foreach (ShopButtonData shopButton in shopButtons)
        {

            Color colorToApply;
            Color colorToApply_Background;
            
            if (pendingTowerToSpawn == shopButton.towerData)
            {
                colorToApply = color_Selected;
                colorToApply_Background = color_Selected_Background;
            }
   
            else if (!PlayerEconomy.Instance.CanBuy(shopButton.towerData.cost))
            {
                colorToApply = color_CantBuy; 
                colorToApply_Background = color_CantBuy_Background;
            }
            
            else
            {
                colorToApply = color_CanBuy; 
                colorToApply_Background = color_CanBuy_Background;
            }
            
            shopButton.buttonBackground.color = colorToApply_Background;
            shopButton.towerIcon.color = colorToApply;
            shopButton.towerName.color = colorToApply;
            shopButton.towerPrice.color = colorToApply;
            shopButton.coin.color = colorToApply;
        }
    }
}