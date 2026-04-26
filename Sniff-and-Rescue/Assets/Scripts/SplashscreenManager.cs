using UnityEngine;
using UnityEngine.InputSystem;

public class SplashscreenManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject sceneControllerObject;

    SceneController sceneController;
    PlayerInput input;
    private InputAction submitInput;

    private void Awake()
    {
        sceneController = sceneControllerObject.GetComponent<SceneController>();
        input = player.GetComponent<PlayerInput>();
        submitInput = input.actions["Submit"];
    }

    private void Update()
    {
        if (submitInput.triggered)
        {
            sceneController.ChangeScene("UI_MainMenu");
        }
    }

}
