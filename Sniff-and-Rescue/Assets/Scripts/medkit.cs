using UnityEngine;

public class medkit : MonoBehaviour, IInteractable
{
	public static int playerScore;

	public void Interact()
	{
		Debug.Log("interacted with medkit");
		playerScore += 100;
		Destroy(this.gameObject);
	}
}
