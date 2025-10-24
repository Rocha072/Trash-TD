using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int life = 100;

    public static PlayerLife Instance { get; private set; }

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

    public int GetLife()
    {
        return life;
    }

    public void TakeDamage(int amount)
    {
        life -= amount;
        UIManager.Instance.UpdateLifeText();

        //if (life <= 0)
        //{
        //    LoseGame();
        //}
    }


}