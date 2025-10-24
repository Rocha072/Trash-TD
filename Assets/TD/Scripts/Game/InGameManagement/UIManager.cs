using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI lifeText;

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

    public void UpdateMoneyText()
    {
        moneyText.text = "" + PlayerEconomy.Instance.GetMoney().ToString();
    }

    public void UpdateLifeText()
    {
        lifeText.text = "" + PlayerLife.Instance.GetLife().ToString();
    }
}