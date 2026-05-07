using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Questions/Question", fileName = "Question")]
public class QuestionData : ScriptableObject
{
    public string question = "Which one of these is correct?";

    public List<string> answers;

    public int correctAnswerIndex;

    public bool IsCorrectAnswer(int answerIndex)
    {
        return answerIndex == correctAnswerIndex;
    }
}
