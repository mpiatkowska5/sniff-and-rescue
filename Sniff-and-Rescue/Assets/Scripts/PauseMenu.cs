using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{


    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject pauseSettings;
    [SerializeField] GameObject player;
    PlayerInput input;

    private void Awake()
    {
        input = player.GetComponent<PlayerInput>();
    }

    void Start()
    {
        pauseMenuUI.SetActive(false);
        pauseSettings.SetActive(false);
        Resume();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
                CloseSettings();
                pauseMenuUI.SetActive(false);
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseSettings.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        pauseSettings.SetActive(true);
    }

    public void CloseSettings()
    {
        pauseMenuUI.SetActive(true);
        pauseSettings.SetActive(false);
    }



    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
