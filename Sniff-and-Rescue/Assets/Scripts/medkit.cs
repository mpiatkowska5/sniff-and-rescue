using UnityEngine;

public class medkit : MonoBehaviour, IInteractable
{
   public void Interact()
   {
	   Debug.Log("interacted with medkit");
   }
}
