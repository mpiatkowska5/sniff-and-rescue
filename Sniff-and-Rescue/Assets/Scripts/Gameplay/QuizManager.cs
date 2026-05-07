using UnityEngine;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private float answerResultDelay = 1f;

    private InGameUIManager uiManager;
    private ScoreManager scoreManager;
    private LevelManager levelManager;
    private QuestionData activeQuestion;
    private Coroutine answerRoutine;

    private void Awake()
    {
        uiManager = GetComponent<InGameUIManager>();
        scoreManager = Resources.Load<ScoreManager>("ScoreManager");
        levelManager = FindFirstObjectByType<LevelManager>();

        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<InGameUIManager>();
        }
    }

    private void OnEnable()
    {
        if (uiManager != null)
        {
            uiManager.AnswerSelected += HandleAnswerSelected;
        }
    }

    private void OnDisable()
    {
        if (uiManager != null)
        {
            uiManager.AnswerSelected -= HandleAnswerSelected;
        }
    }

    public void StartQuiz(QuestionData question)
    {
        if (question == null || uiManager == null)
        {
            return;
        }

        activeQuestion = question;
        levelManager?.SetState(GameState.QuizActive);
        uiManager.DisplayQuiz(question);
    }

    private void HandleAnswerSelected(int answerIndex)
    {
        if (answerRoutine != null)
        {
            return;
        }

        answerRoutine = StartCoroutine(ResolveAnswerAfterDelay(answerIndex));
    }

    private IEnumerator ResolveAnswerAfterDelay(int answerIndex)
    {
        if (activeQuestion != null && activeQuestion.IsCorrectAnswer(answerIndex))
        {
            scoreManager.IncreaseCorrectAnswers();
        }

        if (activeQuestion != null)
        {
            uiManager.ShowAnswerResult(answerIndex, activeQuestion.correctAnswerIndex);
        }

        Time.timeScale = 1f;
        yield return new WaitForSeconds(answerResultDelay);

        activeQuestion = null;
        answerRoutine = null;
        uiManager.CloseQuiz();
        levelManager?.SetState(GameState.Running);
    }
}
