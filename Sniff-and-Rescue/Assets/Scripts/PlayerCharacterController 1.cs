using UnityEngine;
using UnityEngine.InputSystem.XR;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerCharacterController : MonoBehaviour
{
    [Header("General settings")]
    [Tooltip("Layers to be considered as ground")]
    [SerializeField] LayerMask m_groundCheckLayers = Physics.AllLayers;

    [Tooltip("Layers to be considered as obstacles. Needed for standing up from crouching state")]
    [SerializeField] LayerMask m_obstacleLayers = Physics.AllLayers;

    [Tooltip("The FPS camera for the player")]
    [SerializeField] Camera m_playerCamera;

    [Tooltip("Force applied downward when in the air")]
    [SerializeField] float m_gravityDownForce = 20f;

    [Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
    [SerializeField] float m_groundCheckDistance = 0.05f;

    [Header("Movement")]
    [Tooltip("Look rotation speed")]
    [SerializeField] float m_rotationSpeed = 50f;

    [Tooltip("Max movement speed when grounded (when not sprinting)")]
    [SerializeField] float m_maxSpeedOnGround = 10f;

    [Tooltip("Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    [SerializeField] float m_movementSharpnessOnGround = 15;
    
    [Tooltip("Max movement speed when crouching")]
    [SerializeField][Range(0, 1)]
    float m_maxSpeedCrouchedRatio = 0.5f;

    [Tooltip("Max movement speed when not grounded")]
    [SerializeField] float m_maxSpeedInAir = 10f;

    [Tooltip("Acceleration speed when in the air")]
    [SerializeField] float m_accelerationSpeedInAir = 25f;

    [Tooltip("Multiplicator for the sprint speed (based on grounded speed)")]
    [SerializeField] float m_sprintSpeedModifier = 2f;

    [Tooltip("Force applied upward when jumping")]
    [SerializeField] float m_jumpForce = 9f;

    [Tooltip("Ratio (0-1) of the character height where the camera will be at")]
    [SerializeField][Range(0, 1)]
    float m_cameraHeightRatio = 0.9f;

    [Tooltip("Height of character when standing")]
    [SerializeField] float m_capsuleHeightStanding = 1.8f;

    [Tooltip("Height of character when crouching")]
    [SerializeField] float m_capsuleHeightCrouching = 0.9f;

    [Tooltip("Speed of crouching transitions")]
    [SerializeField] float m_crouchingSharpness = 10f;


    Vector3 _characterVelocity;
    Vector3 _groundNormal;
    Vector3 _latestImpactSpeed;

    CharacterController _characterController;
    PlayerInputHandler _inputHandler;

    bool _isGrounded;
    bool _isCrouching;
    bool _hasJumpedThisFrame;
    float _cameraVerticalAngle = 0f;
    float _targetCharacterHeight;
    float _lastTimeJumped = 0f;
    const float k_groundCheckDistanceInAir = 0.12f;
    const float k_jumpGroundingPreventionTime = 0.2f;
    const float k_probeLift = 0.02f; 

    private void Awake()
    {
        // Fetch Components
        _characterController = GetComponent<CharacterController>();
        _inputHandler = GetComponent<PlayerInputHandler>();

        // Remove "Player" from layermasks to avoid confusion with crouching logic etc.
        m_groundCheckLayers = m_groundCheckLayers & ~(1 << gameObject.layer);
        m_obstacleLayers = m_obstacleLayers & ~(1 << gameObject.layer);

    }

    private void Start()
    {
        // Lock the mouse cursor to center of screen
        // and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set start-up settings
        _targetCharacterHeight = m_capsuleHeightStanding;
        UpdateCharacterHeight(true);
    }

    private void Update()
    {
        _hasJumpedThisFrame = false;
        bool wasGrounded = _isGrounded;

        // Are we trying to crouch?
        if (_inputHandler.IsCrouched)
            ForceCrouch();
        else
            TryStandUp();

        // Apply character height this frame
        UpdateCharacterHeight(false);

        // Check for ground
        GroundCheck();

        // Handle movement
        HandleCharacterMovement();
    }

    private void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small,
        // to prevent suddenly snapping to ground
        float chosenGroundCheckDistance =
            _isGrounded ? (_characterController.skinWidth + m_groundCheckDistance) 
            : k_groundCheckDistanceInAir;

        // reset values before the ground check
        _isGrounded = false;
        _groundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump;
        // otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= _lastTimeJumped + k_jumpGroundingPreventionTime)
        {
            
            Vector3 liftedBottom = GetCapsuleBottomHemisphere() + transform.up * k_probeLift;
            Vector3 liftedTop = GetCapsuleTopHemisphere(_characterController.height) + transform.up * k_probeLift;

            // if we're grounded, collect info about the ground normal
            // with a downward capsule cast representing our character capsule
            if (Physics.CapsuleCast(liftedBottom, liftedTop,
                _characterController.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance + k_probeLift,
                m_groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                // storing the upward direction for the surface found
                _groundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f && IsNormalUnderSlopeLimit(_groundNormal))
                {
                    _isGrounded = true;

                    // handle snapping to the ground
                    float snapDistance = hit.distance - k_probeLift;
                    if (snapDistance > _characterController.skinWidth)
                    {
                        _characterController.Move(Vector3.down * snapDistance);
                    }
                }
            }
        }
    }

    private void HandleCharacterMovement()
    {
        // === ROTATIONS ===
        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(
                new Vector3(0f, (_inputHandler.LookInput.x 
                * m_rotationSpeed * Time.deltaTime),
                    0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            _cameraVerticalAngle -= _inputHandler.LookInput.y 
                * m_rotationSpeed * Time.deltaTime;

            // limit the camera's vertical angle to min/max
            _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            m_playerCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
        }

        // === MOVEMENT ===
        bool wantsToSprint = _inputHandler.IsSprinting;
        bool isSprinting = false;
        {
            if (wantsToSprint)
            {
                if (!_isCrouching) // not crouching, can sprint
                    isSprinting = true;
                else // crouching, try to stand up first
                    isSprinting = TryStandUp(false);
            }

            float speedModifier = isSprinting ? m_sprintSpeedModifier : 1f;

            // converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput = transform.TransformVector(_inputHandler.MoveInput);
            bool noMoveInput = worldspaceMoveInput.sqrMagnitude < 1e-6f;


            // handle grounded movement
            if (_isGrounded)
            {

                // calculate the desired velocity from inputs, max speed, and current slope
                Vector3 targetVelocity = worldspaceMoveInput * m_maxSpeedOnGround * speedModifier;
                // reduce speed if crouching by crouch speed ratio
                if (_isCrouching)
                    targetVelocity *= m_maxSpeedCrouchedRatio;
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, _groundNormal) *
                                    targetVelocity.magnitude;

                // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                _characterVelocity = Vector3.Lerp(_characterVelocity, targetVelocity,
                    m_movementSharpnessOnGround * Time.deltaTime);

                if (noMoveInput && _characterVelocity.magnitude < 0.05f)
                    _characterVelocity = Vector3.zero;

                // Kill vertical drift
                if (!_hasJumpedThisFrame)
                    _characterVelocity.y = 0f;

                // === JUMPING ===
                if (_isGrounded && _inputHandler.JumpQueued)
                {
                    // force the crouch state to false
                    //if (SetCrouchingState(false, false))
                    if (!_isCrouching || TryStandUp(false))
                    {
                        // start by canceling out the vertical component of our velocity
                        _characterVelocity = new Vector3(_characterVelocity.x, 0f, _characterVelocity.z);

                        // then, add the jumpSpeed value upwards
                        _characterVelocity += Vector3.up * m_jumpForce;

                        // remember last time we jumped because we need to prevent snapping to ground for a short time
                        _lastTimeJumped = Time.time;
                        _hasJumpedThisFrame = true;

                        // Force grounding to false
                        _isGrounded = false;
                        _groundNormal = Vector3.up;
                    }
                    _inputHandler.JumpQueued = false;
                }

            }
            // handle air movement
            else
            {
                // add air acceleration
                _characterVelocity += worldspaceMoveInput * m_accelerationSpeedInAir * Time.deltaTime;

                // limit air speed to a maximum, but only horizontally
                float verticalVelocity = _characterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_characterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, m_maxSpeedInAir * speedModifier);
                _characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // apply the gravity to the velocity
                _characterVelocity += Vector3.down * m_gravityDownForce * Time.deltaTime;
            }
        }

        // apply the final calculated velocity value as a character movement
        Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
        Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(_characterController.height);
        _characterController.Move(_characterVelocity * Time.deltaTime);

        // detect obstructions to adjust velocity accordingly
        _latestImpactSpeed = Vector3.zero;
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, _characterController.radius,
            _characterVelocity.normalized, out RaycastHit hit, _characterVelocity.magnitude * Time.deltaTime,
            m_obstacleLayers, QueryTriggerInteraction.Ignore))
        {
            // We remember the last impact speed because the fall damage logic might need it
            _latestImpactSpeed = _characterVelocity;

            _characterVelocity = Vector3.ProjectOnPlane(_characterVelocity, hit.normal);
        }
    }

    // === Crouching related ===

    public bool CanStandUp()
    {
        const float lift = 0.01f;
        Vector3 bottom = GetCapsuleBottomHemisphere() + transform.up * lift;
        Vector3 top = GetCapsuleTopHemisphere(m_capsuleHeightStanding) + transform.up * lift;
        float r = _characterController.radius;

        bool blocked = Physics.CheckCapsule(bottom, top, r, m_obstacleLayers, QueryTriggerInteraction.Ignore);
        return !blocked;
    }
    public void ForceCrouch()
    {
        if (!_isCrouching)
        {
            _isCrouching = true;
            _targetCharacterHeight = m_capsuleHeightCrouching;
        }
    }

    public bool TryStandUp(bool ignoreObstructions = false)
    {
        if (!_isCrouching) return true; // already standing

        if (ignoreObstructions || CanStandUp()) // must or can stand up
        {
            _isCrouching = false;
            _targetCharacterHeight = m_capsuleHeightStanding;
            return true;
        }
        return false;
    }


    // === Utility methods ===

    private void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            _characterController.height = _targetCharacterHeight;
            _characterController.center = Vector3.up * _characterController.height * 0.5f;
            m_playerCamera.transform.localPosition = Vector3.up * _targetCharacterHeight * m_cameraHeightRatio;
        }
        // Update smooth height
        else if (_characterController.height != _targetCharacterHeight)
        {
            // resize the capsule and adjust camera position
            _characterController.height = Mathf.Lerp(_characterController.height, _targetCharacterHeight,
                m_crouchingSharpness * Time.deltaTime);
            _characterController.center = Vector3.up * _characterController.height * 0.5f;
            m_playerCamera.transform.localPosition = Vector3.Lerp(m_playerCamera.transform.localPosition,
                Vector3.up * _targetCharacterHeight * m_cameraHeightRatio, m_crouchingSharpness * Time.deltaTime);
        }
    }

    // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= _characterController.slopeLimit;
    }

    // Gets the center point of the bottom hemisphere of the character controller capsule    
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * _characterController.radius);
    }

    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - _characterController.radius));
    }

    // Gets a reoriented direction that is tangent to a given slope
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

}
