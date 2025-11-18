using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEditor;
public class MenuManagement : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelMenuPrincipal;
    public GameObject panelSelectLevel;
    public GameObject panelGuide;
    public GameObject panelLoading;

    [Header("Loading Screen")]
    public Slider sliderLoading;
    public TextMeshProUGUI txtPorcentage;

    [Header("Music")]
    public AudioClip menuMusic;
    void Start()
    {
        BackToMainMenu();
        SoundHandler.Instance.PlayMusic(menuMusic, 0.1f);
    }


    public void BackToMainMenu()
    {
        panelMenuPrincipal.SetActive(true);
        panelSelectLevel.SetActive(false);
        panelGuide.SetActive(false);
        panelLoading.SetActive(false);
    }

    public void PlayButton()
    {
        panelMenuPrincipal.SetActive(false);
        panelSelectLevel.SetActive(true);
        panelGuide.SetActive(false);
        panelLoading.SetActive(false);
    }

    public void GuideButton()
    {
        panelMenuPrincipal.SetActive(false);
        panelSelectLevel.SetActive(false);
        panelGuide.SetActive(true);
        panelLoading.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SelectLevel(int LevelNumber)
    {
        panelMenuPrincipal.SetActive(false);
        panelSelectLevel.SetActive(false);
        panelGuide.SetActive(false);
        panelLoading.SetActive(true);
        StartCoroutine(LoadScene(LevelNumber));

    }
    
    private IEnumerator LoadScene(int SceneNumber)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneNumber);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f) * 100;

            sliderLoading.value = progress;
            txtPorcentage.text = progress + "%";

            yield return null;
        }
    }
}
