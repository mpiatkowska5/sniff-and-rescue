using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseSettings;
    [SerializeField] private GameObject contBtn;
    [SerializeField] private GameObject player;

    PlayerInput input;
    private InputAction pauseInput;
    private InputAction cancelInput;
    EventSystem eventSystem;

    void Awake()
    {
        input = player.GetComponent<PlayerInput>();
        pauseInput = input.actions["Pause"];
        cancelInput = input.actions["Cancel"];
        eventSystem = EventSystem.current;
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

        if (cancelInput.triggered) 
        {
            Debug.Log("Cancel input pressed");
            if (GameIsPaused)
            {
                if (pauseMenuUI.activeSelf == true)
                {
                    Resume();
                }
                else if (pauseSettings.activeSelf == true) 
                {
                    CloseSettings();
                }
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
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        eventSystem.SetSelectedGameObject(contBtn.gameObject);
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
        eventSystem.SetSelectedGameObject(contBtn.gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
