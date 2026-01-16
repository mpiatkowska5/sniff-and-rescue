using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float runningSpeed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float mouseSensitivity = 100f;

    [SerializeField] Transform spawningPoint;

    //using the whole body as the "head" rn, later can be changed to a dog head model (to have the sount visible) and we can have the paws separately
    [SerializeField] Transform head;

    float xRotation; //remember head rotation
    Vector3 velocity; // remember player speed (need for falling)

     //retrieved from InputSystem
    public Vector2 moveInput;
    Vector3 lookInput;
    float isRunning;
    float speed;

    CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // deal with mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        speed = walkingSpeed;
    }

    private void Start()
    {
       // this.ResetBackToSpawningPoint();
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
        if(controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void OnRunStart(InputValue value)
    {
        isRunning = value.Get<float>();
        speed = runningSpeed;
        Debug.Log(speed);
    }

    private void OnRunStop()
    {
        speed = walkingSpeed;    
        Debug.Log(speed);
        
    }

    private void Update()
    {
        /*if(isRunning>0.2)
        {
            speed = runningSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }*/
        
        // === LOOKING ===
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        //Look up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //turn 
        transform.Rotate(Vector3.up * mouseX);
      
        // == GRAVITY ==
        //apply pull of gravity every frame
        velocity.y += gravity * Time.deltaTime;

         //if we are standing on ground, reset velocity.y
        if(controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //clamp feet to the ground
        }

        // == MOVING ==
        //translate input into 3D vector
        Vector3 movement = (transform.right * moveInput.x 
                         + transform.forward * moveInput.y).normalized;

        // Apply Movement
        controller.Move((movement * speed + velocity) * Time.deltaTime);
        
    }

    public void ResetBackToSpawningPoint()
    {
        controller.enabled = false;
        transform.position = spawningPoint.position;
        controller.enabled = true;

    }

}



