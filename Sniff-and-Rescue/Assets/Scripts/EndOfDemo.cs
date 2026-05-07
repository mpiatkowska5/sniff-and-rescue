using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDemo : MonoBehaviour
{
    private ScoreManager scoreManager;
    private LevelManager levelManager;

    private void Awake()
    {
        scoreManager = Resources.Load<ScoreManager>("ScoreManager");
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit end trigger");
            GameEnd();
            levelManager.EndLevel();
            //SceneManager.LoadScene(sceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void GameEnd()
    {
        //currently the player gets "1000 - minus their time" points at the end of the game
        //can find a different way of counting the score after playtesting

        int timeScoreDeduct = Mathf.FloorToInt(GameplayManager.time);
        int timeScore = 1000 - timeScoreDeduct;
        scoreManager.IncreaseScore(timeScore);
    }
}
