using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity= 9.81f;


    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2.0f; 
    [SerializeField] private float upDownRange = 80.0f;

    private CharacterController characterController; 
    private Camera mainCamera;
    private PlayerMovement inputHandler;
    private Vector3 currentMovement;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); 
        mainCamera = GetComponent<Camera>();
        //inputHandler = PlayerMovement.Instance;
        inputHandler = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        Movement();
        //Rotation();
    }

    void Movement()
    {
        
    Vector3 horizontalMovement = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y); 
    horizontalMovement = transform.forward * horizontalMovement.z + transform.right * horizontalMovement.x; 
    horizontalMovement. Normalize();

    float speed = walkSpeed * (inputHandler.SprintValue > 0 ? sprintMultiplier: 1f);
    currentMovement.x = horizontalMovement.x * speed;
    currentMovement.z = horizontalMovement.z * speed;
    HandleJumping();
    characterController. Move (currentMovement*Time.deltaTime);
    }

    void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (inputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }
    void Rotation()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.GetChild(1).transform.position);
        dir.Normalize();
        var angle1 = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.GetChild(1).transform.rotation = Quaternion.AngleAxis(angle1*-1, Vector3.up);
        
    }
}
