using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame() //go to: file->build profiles, add main menu (1) + game scene (0) 
    {
        SceneManager.LoadScene(0);
    }

   public void QuitGame()
    {
        Application.Quit();
    }    
}
