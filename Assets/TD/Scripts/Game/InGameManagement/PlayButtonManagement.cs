using UnityEngine;
using UnityEngine.UI;

public class PlayButtonManagement : MonoBehaviour
{
    [Header("Objetos icon e glow effect")]
    [SerializeField] private Image buttonIcon;
    [SerializeField] private GameObject glowEffect;

    [Header("Sprites")]
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite fastFowardIcon;

    private enum ButtonState
    {
        Play, NormalSpeed, FastSpeed
    }

    private ButtonState currentButtonState;

    private WaveManager waveManager;

    private Button thisButton;

    void Start()
    {
        waveManager = WaveManager.Instance;
        thisButton = GetComponent<Button>();
    }

    void Update()
    {
        CheckWaveManagerState();
        if (!UIManager.Instance.pauseScreen.activeSelf)
        {
            thisButton.interactable = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeButtonState();
            }
        }
        else
        {
            thisButton.interactable = false;
        }
    }

    void CheckWaveManagerState()
    {
        if (waveManager.currentState == WaveManager.WaveState.WaitingToStart)
        {
            if (currentButtonState != ButtonState.Play)
                SetState(ButtonState.Play);
        }
    }


    public void ChangeButtonState()
    {
        switch (currentButtonState)
        {
            case ButtonState.Play:
                waveManager.StartNextWave();
                SetState(ButtonState.NormalSpeed);
                Time.timeScale = 1.0f;
                break;

            case ButtonState.NormalSpeed:
                SetState(ButtonState.FastSpeed);
                Time.timeScale = 2.0f;
                break;

            case ButtonState.FastSpeed:
                SetState(ButtonState.NormalSpeed);
                Time.timeScale = 1.0f;
                break;
        }
    }
    
    private void SetState(ButtonState newState)
    {
        currentButtonState = newState;
        
        switch (newState)
        {
            case ButtonState.Play:
                buttonIcon.sprite = playIcon;
                glowEffect.SetActive(false);
                Time.timeScale = 1.0f;
                break;

            case ButtonState.NormalSpeed:
                buttonIcon.sprite = fastFowardIcon;
                glowEffect.SetActive(false);
                break;

            case ButtonState.FastSpeed:
                buttonIcon.sprite = fastFowardIcon;
                glowEffect.SetActive(true);
                break;
        }
    }


}
