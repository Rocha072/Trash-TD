using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("Lifes/Money")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI lifeText;

    [Header("Mouse and Pause Screen")]
    public MousePosition mouse;
    public GameObject pauseScreen;
    public GameObject gameGuide;
    public GameObject optionsScreen;

    [Header("Shop Bar")]
    public GameObject Shop;
    public CanvasGroup ShopCanvasGroup;


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
                PlayerMovement.TurnOnPlayerMovement();
                Shop.SetActive(false);
            }
            else
            {
                PlayerMovement.TurnOffPlayerMovement();
                Shop.SetActive(true);
                
            }

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
        Time.timeScale = 1f;
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
}