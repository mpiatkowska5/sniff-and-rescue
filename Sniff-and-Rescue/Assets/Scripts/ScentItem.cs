using UnityEngine;

public class ScentItem : MonoBehaviour, IInteractable
{
    [SerializeField] ParticleSystem scentTrail;

    public void Interact()
    {
        scentTrail.Play();
    }
}
