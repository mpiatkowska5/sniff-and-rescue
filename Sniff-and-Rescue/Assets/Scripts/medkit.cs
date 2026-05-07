using UnityEngine;

public class medkit : MonoBehaviour, IInteractable
{
	public static int playerScore;
	public static int medKitsCollected = 0;

    [SerializeField] QuestionData question;
    [SerializeField] QuizManager quizManager;
    private ScoreManager scoreManager;

    private void Awake()
    {
        if (quizManager == null)
        {
            quizManager = FindFirstObjectByType<QuizManager>();
        }
        scoreManager = Resources.Load<ScoreManager>("ScoreManager");
    }

    public void Interact()
	{
		Debug.Log("interacted with medkit");
        scoreManager.IncreaseScore(50);
		medKitsCollected++;
        if (quizManager == null)
        {
            InGameUIManager uiManager = FindFirstObjectByType<InGameUIManager>();
            quizManager = uiManager != null ? uiManager.gameObject.AddComponent<QuizManager>() : null;
        }

        if (quizManager != null)
        {
            quizManager.StartQuiz(question);
            Destroy(gameObject);
        }
        //Destroy(this.gameObject);
	}
}
