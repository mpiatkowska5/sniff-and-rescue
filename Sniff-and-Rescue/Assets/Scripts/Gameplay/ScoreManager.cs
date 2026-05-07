using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/Score Manager")]
public class ScoreManager : ScriptableObject
{
    public int Score { get; private set; }
    public int CorrectAnswers { get; private set; }
    public int CorrentAnswers => CorrectAnswers;

    public event Action onScoreChanged;
    public event Action onCorrectAnswerChanged;

    public void IncreaseScore(int amountToAdd)
    {
        Score += amountToAdd;
        onScoreChanged?.Invoke();
    }

    public void IncreaseCorrectAnswers()
    {
        CorrectAnswers += 1;
        onCorrectAnswerChanged?.Invoke();
    }

    public void Reset()
    {
        Score = 0;
        CorrectAnswers = 0;
        onScoreChanged?.Invoke();
        onCorrectAnswerChanged?.Invoke();
    }
}
