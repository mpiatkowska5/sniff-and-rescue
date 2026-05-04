using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float runningSpeed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float speed = 5f;

    //using the whole body as the "head" rn, later can be changed to a dog head model (to have the sount visible) and we can have the paws separately
    [SerializeField] Transform head;

    float xRotation; //remember head rotation
    Vector3 velocity; // remember player speed

    public static bool jumpUnlocked;
    bool canJump;
    bool isRunning;
    

     //retrieved from InputSystem
    public Vector2 moveInput;
    private Vector3 movement;
    private Vector3 lookInput;

    CharacterController controller;

    PlayerInput input;
    //[SerializeField] Rigidbody rb;
    


    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        //rb = GetComponentInChildren<Rigidbody>();

        input = GetComponent<PlayerInput>();
        // deal with mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //block jump until trigger
        canJump = false;
        isRunning = false;
        jumpUnlocked = false;

    }
    
    //get the input from keyboard/gamepad for movement
    private void OnMove(InputValue value)
    {
        moveInput  = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void OnJump()
    {
        if (jumpUnlocked == true)
        {
            jumpUnlocked = false;
        }
        if(controller.isGrounded && canJump)
        {
            //velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            velocity = new Vector3(velocity.x, 0f, velocity.z);
            velocity += Vector3.up * jumpForce;
        }

    }

    private void OnRunStart(InputValue value)
    {
        //isRunning = value.Get<float>();
        if (isRunning)
        {
            speed = walkingSpeed;
            isRunning = false;
        }else
        {
            speed = runningSpeed;
            isRunning=true;
        }
            
        Debug.Log(speed);
    }

    private void Update()
    {        
        // === LOOKING ===
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        //Look up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //turn 
        transform.Rotate(new Vector3(0f, mouseX, 0f), Space.Self);

        // == GRAVITY ==
        //apply pull of gravity every frame
        velocity.y += gravity * Time.deltaTime;

        // == MOVING ==
        //translate input into 3D vector
        movement = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

        //speed = 8 * moveInput.magnitude;

        // Apply Movement
        //controller.Move((movement * (speed * moveInput.magnitude) + velocity) * Time.deltaTime);
        controller.Move((movement * (speed) + velocity) * Time.deltaTime);

        //Debug.Log(speed);
        
    }

    private void OnTriggerEnter(Collider unlockCollider)
    {
        if (unlockCollider.gameObject.CompareTag("JumpUnlock"))
        {
            Destroy(unlockCollider);
            Debug.Log("Should enable jump");
            canJump = true;
            jumpUnlocked = true;
        }
    }

    public void SetParent(Transform newParent)
    {
        transform.parent = newParent;
    }

    public void Respawn(Transform spawnPoint)
    {
        controller.enabled = false;
        Debug.Log(spawnPoint);
        transform.position = spawnPoint.position;
        controller.enabled = true;
        //transform.position = spawnPoint.position;
    }


}



