using UnityEngine;

public class medkit : MonoBehaviour, IInteractable
{
	public static int playerScore;
	public static int medKitsCollected = 0;

	public void Interact()
	{
		Debug.Log("interacted with medkit");
		playerScore += 100;
		medKitsCollected++;
		Destroy(this.gameObject);
	}
}
