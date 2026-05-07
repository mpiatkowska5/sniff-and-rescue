using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    private ScoreManager scoreManager;
    private InGameUIManager uiManager;

    public GameState CurrentState { get; private set; } = GameState.Running;
    public event Action<GameState> StateChanged;

    private void Awake()
    {
        scoreManager = Resources.Load<ScoreManager>("ScoreManager");
        uiManager = FindFirstObjectByType<InGameUIManager>();
    }

    private void Start()
    {
        scoreManager.Reset();
        SetState(GameState.Running);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void SetState(GameState state)
    {
        if (CurrentState == state)
        {
            return;
        }

        CurrentState = state;
        StateChanged?.Invoke(CurrentState);

        if (uiManager != null)
        {
            uiManager.DisplayState(CurrentState);
        }
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.GameEnded || CurrentState == GameState.QuizActive)
        {
            return;
        }

        SetState(CurrentState == GameState.Paused ? GameState.Running : GameState.Paused);
    }

    public void EndLevel()
    {
        SetState(GameState.GameEnded);
    }
}
