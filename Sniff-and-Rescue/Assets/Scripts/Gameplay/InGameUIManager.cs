using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InGameUIManager : MonoBehaviour
{

    [Header("Hud")]
    //[SerializeField] TMP_Text correctAnswersTextObject;

    [Header("Screens")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject endScreen;

    [Header("Quiz")]
    [SerializeField] GameObject quizScreen;
    [SerializeField] TMP_Text questionTextObject;
    [SerializeField] GameObject answerButtonContainer;
    [SerializeField] Button prefabAnswerButton;
    [SerializeField] Color correctAnswerColor = new Color(0.2f, 0.75f, 0.25f);
    [SerializeField] Color wrongAnswerColor = new Color(0.9f, 0.2f, 0.2f);
    [SerializeField] Color defaultAnswerColor = Color.white;
    private List<Button> answerButtons;



    private ScoreManager scoreManager;
    EventSystem eventSystem;
    public event Action<int> AnswerSelected;


    private void Awake()
    {
        scoreManager = Resources.Load<ScoreManager>("ScoreManager");

        eventSystem = EventSystem.current;

        answerButtons = new List<Button>();
    }
    private void OnEnable()
    {
        scoreManager.onCorrectAnswerChanged += UpdateCorrectAnswers;
    }
    private void OnDisable()
    {
        scoreManager.onCorrectAnswerChanged -= UpdateCorrectAnswers;
    }

    private void Start()
    {
        UpdateCorrectAnswers();
        CloseQuiz();
        DisplayState(GameState.Running);

    
    }

    private void UpdateCorrectAnswers()
    {
        //correctAnswersTextObject.text = $"Correct answers: {scoreManager.CorrectAnswers}";
    }

    public void DisplayQuiz(QuestionData question)
    {
        ClearAnswerButtons();

        quizScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        questionTextObject.text = question.question;

        for (int i = 0; i < question.answers.Count; i++)
        {
            int buttonIndex = i;
            var button = Instantiate(prefabAnswerButton, answerButtonContainer.transform);
            var buttonText = button.GetComponentInChildren<TMP_Text>();
            ResetAnswerButtonVisual(button);
            button.onClick.AddListener(() => OnAnswerButtonPressed(buttonIndex));

            if (buttonText != null)
            {
                buttonText.text = question.answers[buttonIndex];
            }

            answerButtons.Add(button);
        }
        eventSystem.SetSelectedGameObject(answerButtons[0].gameObject);
    }

    public void ShowAnswerResult(int selectedAnswerIndex, int correctAnswerIndex)
    {
        for (int i = 0; i < answerButtons.Count; i++)
        {
            Button button = answerButtons[i];
            button.interactable = false;

            if (i == correctAnswerIndex)
            {
                SetAnswerButtonColor(button, correctAnswerColor);
             
            }
            else if (i == selectedAnswerIndex)
            {
                SetAnswerButtonColor(button, wrongAnswerColor);
               
            }
        }
    }

    public void CloseQuiz()
    {
        ClearAnswerButtons();
        quizScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    private void ClearAnswerButtons()
    {
        foreach(Transform t in answerButtonContainer.transform)
        {
            if (t.TryGetComponent(out Button button))
            {
                button.onClick.RemoveAllListeners();
            }

            Destroy(t.gameObject);
        }

        answerButtons.Clear();
    }

    public void DisplayState(GameState state)
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(state == GameState.Paused);
        }

        if (endScreen != null)
        {
            endScreen.SetActive(state == GameState.GameEnded);
        }
    }

    private void ResetAnswerButtonVisual(Button button)
    {
        button.interactable = true;
        SetAnswerButtonColor(button, defaultAnswerColor);
    }

    private void SetAnswerButtonColor(Button button, Color color)
    {
        if (button.targetGraphic != null)
        {
            button.targetGraphic.color = color;
        }
    }

    private void OnAnswerButtonPressed(int buttonIndex)
    {
        AnswerSelected?.Invoke(buttonIndex);
    }
}
