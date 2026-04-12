using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{    
    private void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }
   
    public void PlayGame() //go to: file->build profiles, add main menu (1) + game scene (0) 
    {
        SceneManager.LoadScene(0);
        MusicManager.Instance.PlayMusic("Game - wind");
       
    }

   public void QuitGame()
    {
        Application.Quit();
    }    

}
