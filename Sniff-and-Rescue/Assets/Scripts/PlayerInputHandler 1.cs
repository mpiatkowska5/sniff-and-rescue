using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCharacterController))]
public class PlayerInputHandler : MonoBehaviour
{
    // === Cached components ===
    /* Player related components this input handler
     * needs to know about */
    private PlayerCharacterController _playerCharacterController;

    // === FIELDS ===
    /* Backing fields */
    private Vector3 _moveInput;
    private Vector2 _lookInput;
    private bool _isCrouched;
    private bool _isSprinting;

    /* Public properties */
    public Vector3 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;
  
    public bool JumpQueued { get; set; }
    public bool IsCrouched => _isCrouched;
    public bool IsSprinting => _isSprinting;

    // === Input actions ===
    /* Lists all of the inputs relevant to the player */
    private PlayerInput _input;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _jumpAction;
    private InputAction _interactAction;
    private InputAction _crouchAction;


    private void OnEnable()
    {
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _crouchAction.started += OnCrouch;
        _crouchAction.canceled += OnCrouch;
        _crouchAction.performed += OnCrouch;
        _jumpAction.performed += OnJump;
        _lookAction.performed += OnLook;
        _lookAction.canceled += OnLook;
        _sprintAction.started += OnSprint;
        _sprintAction.canceled += OnSprint;

        _moveAction.Enable();
        _crouchAction.Enable();
        _jumpAction.Enable();
        _lookAction.Enable();
        _sprintAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _crouchAction.started -= OnCrouch;
        _crouchAction.canceled -= OnCrouch;
        _crouchAction.performed -= OnCrouch;
        _jumpAction.performed -= OnJump;
        _lookAction.performed -= OnLook;
        _lookAction.canceled -= OnLook;
        _sprintAction.started -= OnSprint;
        _sprintAction.canceled -= OnSprint;

        _moveAction.Disable();
        _crouchAction.Disable();
        _jumpAction.Disable();
        _lookAction.Disable();
        _sprintAction.Disable();
    }

    private void Awake()
    {
        // Fetch Components
        _input = GetComponent<PlayerInput>();
        _playerCharacterController = GetComponent<PlayerCharacterController>();

        // Fetch Inputs
        _moveAction = _input.actions["Move"];
        _lookAction = _input.actions["Look"];
        _sprintAction = _input.actions["Sprint"];
        _jumpAction = _input.actions["Jump"];
        _interactAction = _input.actions["Interact"];
        _crouchAction = _input.actions["Crouch"];
    }

    // ======================
    // === INPUT HANDLING ===
    // ======================
    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector3 move = new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y);
        move = Vector3.ClampMagnitude(move, 1);
        _moveInput = move;
    }
    private void OnLook(InputAction.CallbackContext ctx) => _lookInput = ctx.ReadValue<Vector2>();
    private void OnJump(InputAction.CallbackContext ctx) => JumpQueued = true;
    private void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            _isSprinting = true;
        else if (ctx.canceled)
            _isSprinting = false;
    }
    private void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            _isCrouched = true;
        else if (ctx.canceled)
            _isCrouched = false;
    }
}
