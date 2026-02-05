using UnityEngine;
using System.Collections;

public class ScentItem : MonoBehaviour, IInteractable
{
    //the scent trail activated by this item
    [SerializeField] ParticleSystem scentTrail;

    //amount of time in seconds during which the scent trail is visible
    [SerializeField] float waitTime;

    public void Interact()
    {
        Debug.Log("Interacted with scent item");
        StartScentTrail();
    }

    void StartScentTrail()
    {
        scentTrail.Play();
        StartCoroutine(StopScentTrail());
    }

    IEnumerator StopScentTrail()
    {
        yield return new WaitForSeconds(waitTime);
        scentTrail.Stop();
    }
}
