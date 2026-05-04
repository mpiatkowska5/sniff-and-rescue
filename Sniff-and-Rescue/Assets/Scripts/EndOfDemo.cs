using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDemo : MonoBehaviour
{
    [SerializeField] private string sceneName = "UI_Menu_Final";
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit end trigger");
            GameEnd();
            SceneManager.LoadScene(sceneName);
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
        Debug.Log(timeScoreDeduct);
        Debug.Log(medkit.playerScore);
        medkit.playerScore += timeScore;
        Debug.Log(medkit.playerScore);

    }
}
