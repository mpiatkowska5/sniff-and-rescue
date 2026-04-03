using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{


    [Header("Buttons")]
    [SerializeField] Button NewGameButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button ExitSettingsButton;
    [SerializeField] Button QuitButton;

    [Header("Screens")]
    [SerializeField] GameObject MainMenuScreen;
    [SerializeField] GameObject SettingsScreen;

    [Header("Scenes")]
    [SerializeField] SceneAsset FirstLevelScene;


    [Header("Fade-out overlay")]
    [SerializeField] CanvasGroup fadeoutCanvasGroup;
    [SerializeField] float fadeOutTime = 1.0f;



    private void Awake()
    {

        MainMenuScreen.SetActive(true);
        SettingsScreen.SetActive(false);


        NewGameButton.onClick.AddListener(() => LoadFirstScene());
        settingsButton.onClick.AddListener(() => OpenSettingsScreen());
        ExitSettingsButton.onClick.AddListener(() => OpenMainMenu());
        QuitButton.onClick.AddListener(() => StartCoroutine(QuitGame()));
    }


    IEnumerator FadeOut(float duration)
    {

        if (EventSystem.current != null)
        {
            EventSystem.current.enabled = false;
        }

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeoutCanvasGroup.alpha = timer / duration;
            yield return null;
        }
        fadeoutCanvasGroup.alpha = 1.0f;

    }






    private void LoadFirstScene()
    {

        SceneManager.LoadScene(FirstLevelScene.name);

    }

    private void OpenSettingsScreen()
    {
        MainMenuScreen.SetActive(false);
        SettingsScreen.SetActive(true);
    }

    private void OpenMainMenu()
    {
        SettingsScreen.SetActive(false);
        MainMenuScreen.SetActive(true);
    }

    private IEnumerator QuitGame()
    {
        yield return StartCoroutine(FadeOut(fadeOutTime));

        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();

    }




}
