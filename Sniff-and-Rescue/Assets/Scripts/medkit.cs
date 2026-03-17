using UnityEngine;

public class medkit : MonoBehaviour, IInteractable
{
	public int playerScore;

   public void Interact()
   {
	   Debug.Log("interacted with medkit");
	   playerScore = playerScore + 10;
	   Destroy(this.gameObject);
   }
}
