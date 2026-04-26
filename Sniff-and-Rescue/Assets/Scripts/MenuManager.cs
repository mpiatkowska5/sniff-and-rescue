using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject menu;
    [SerializeField] private Button playBtn;

    PlayerInput input;
    private InputAction cancelInput;
    EventSystem eventSystem;

    private void Awake()
    {
        input = player.GetComponent<PlayerInput>();
        cancelInput = input.actions["Cancel"];
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (cancelInput.triggered)
        {
            if (settings.activeSelf == true)
            {
                menu.SetActive(true);
                eventSystem.SetSelectedGameObject(playBtn.gameObject);
                settings.SetActive(false);
            }
        }
    }
}
