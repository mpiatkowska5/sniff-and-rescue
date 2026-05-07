using UnityEngine;
using UnityEngine.InputSystem;

namespace Quiztastic.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float gravity = -20f;

        private CharacterController controller;
        private InputAction moveAction;
        private LevelManager levelManager;
        private float verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            levelManager = FindFirstObjectByType<LevelManager>();

            moveAction = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
            moveAction.AddBinding("<Gamepad>/leftStick");
        }

        private void OnEnable()
        {
            moveAction.Enable();
        }

        private void OnDisable()
        {
            moveAction.Disable();
        }

        private void OnDestroy()
        {
            moveAction.Dispose();
        }

        private void Update()
        {
            if (levelManager != null && levelManager.CurrentState != GameState.Running)
                return;

            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 movement = new Vector3(input.x, 0f, input.y);

            if (movement.sqrMagnitude > 1f)
            {
                movement.Normalize();
            }

            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -1f;
            }

            verticalVelocity += gravity * Time.deltaTime;
            movement = movement * moveSpeed + Vector3.up * verticalVelocity;
            controller.Move(movement * Time.deltaTime);
        }
    }
}
