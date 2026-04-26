using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    public GameObject pauseSettings;
    [SerializeField] GameObject player;
    PlayerInput input;
    private InputAction pauseInput;

    void Awake()
    {
        input = player.GetComponent<PlayerInput>();
        pauseInput = input.actions["Pause"];
        Resume();
    }

    private void Update()
    {
        if (pauseInput.triggered)
        {
            Debug.Log("Pause pressed");
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        pauseSettings.SetActive(false);
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseSettings.SetActive(false);
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
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
