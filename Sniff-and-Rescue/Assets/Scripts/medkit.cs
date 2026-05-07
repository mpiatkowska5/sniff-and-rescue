using UnityEngine;

public class medkit : MonoBehaviour, IInteractable
{
	public static int playerScore;
	public static int medKitsCollected = 0;

    [SerializeField] QuestionData question;
    [SerializeField] QuizManager quizManager;

    private void Awake()
    {
        if (quizManager == null)
        {
            quizManager = FindFirstObjectByType<QuizManager>();
        }
    }

    public void Interact()
	{
		Debug.Log("interacted with medkit");
		playerScore += 100;
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
