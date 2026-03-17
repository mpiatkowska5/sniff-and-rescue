using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactionDistance;
    IInteractable currentInteractable;
    [SerializeField] GameObject interactionPrompt;

    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    void Update()
    {
        UpdateInteractable();
    }

    private void UpdateInteractable()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f,0.5f));

        Physics.Raycast(ray, out var objectHit, interactionDistance);

        if(objectHit.collider != null)
        {
            currentInteractable = objectHit.collider.GetComponent<IInteractable>();
        }
        else
        {
            currentInteractable = null;
        }

        if(currentInteractable != null)
        {
            interactionPrompt.SetActive(true);
        } else
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void OnInteract()
    {
        if (currentInteractable != null ){
            currentInteractable.Interact();
        }
    }


}
