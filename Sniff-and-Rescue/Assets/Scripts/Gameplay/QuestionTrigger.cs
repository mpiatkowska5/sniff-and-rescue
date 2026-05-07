using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    [SerializeField] QuestionData question;
    [SerializeField] QuizManager quizManager;

    private void Awake()
    {
        if (quizManager == null)
        {
            quizManager = FindFirstObjectByType<QuizManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
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
        }
    }

}
