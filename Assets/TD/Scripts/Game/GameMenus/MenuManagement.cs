using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManagement : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelSelectLevel;
    public GameObject panelGuide;
    void Start()
    {
        BackToMainMenu();
    }


    public void BackToMainMenu()
    {
        panelMenuPrincipal.SetActive(true);
        panelSelectLevel.SetActive(false);
        panelGuide.SetActive(false);
    }

    public void PlayButton()
    {
        panelMenuPrincipal.SetActive(false);
        panelSelectLevel.SetActive(true);
        panelGuide.SetActive(false);
    }

    public void GuideButton()
    {
        panelMenuPrincipal.SetActive(false);
        panelSelectLevel.SetActive(false);
        panelGuide.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SelectLevel(int LevelNumber)
    {
        SceneManager.LoadScene(LevelNumber);
    }
}
