using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    public static float time;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text medkitText;
    [SerializeField] GameObject tipText;
    [SerializeField] GameObject runTipText;
    [SerializeField] GameObject endScreen;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject InteractPrompt;
 
    [SerializeField] private GameObject player;

    PlayerInput input;
    private InputAction submitInput;

    private LevelManager levelManager;
    private ScoreManager scoreManager;
    private SceneController sceneController;

    private void Awake()
    {
        tipText.SetActive(false);
        endScreen.SetActive(false);
        InteractPrompt.SetActive(true);

        input = player.GetComponent<PlayerInput>();
        submitInput = input.actions["Submit"];

        levelManager = FindFirstObjectByType<LevelManager>();
        sceneController = FindFirstObjectByType<SceneController>();
        scoreManager = Resources.Load<ScoreManager>("ScoreManager");
    }

    void Update()
    {
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        medkitText.text = string.Format($"{medkit.medKitsCollected}/8");
        ActivateJumpTip();
        ActivateRunTip();

        if(levelManager.CurrentState == GameState.GameEnded)
        {
            ActivateEndScreen();
            if (submitInput.triggered)
            {
                sceneController.ChangeScene("UI_Menu_Final");
            }
        }

    }

    private void ActivateJumpTip()
    {
        if (PlayerController.jumpUnlocked == true)
        {
            tipText.SetActive(true);
        }
        else
        {
            tipText.SetActive(false);
        }
    }

    private void ActivateRunTip()
    {
        if(PlayerController.runTipShouldShow == true)
        {
            runTipText.SetActive(true);
        }
        else
        {
            runTipText.SetActive(false);
        }
    }

    private void ActivateEndScreen()
    {
        endScreen.SetActive(true);
        scoreText.text = scoreManager.Score.ToString();
        InteractPrompt.SetActive(false);
    }
}
