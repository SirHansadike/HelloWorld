//most of movement script from Comp-3 Interactive player movement tutorial
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayaMoveScript: MonoBehaviour
{
    public bool canMove { get; private set; } = true;

    //declaring adjustable variables for character behavior
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float gravity = 30.0f;

    //look sensitivity and range of motion for look controls
    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    //declaring methods for movement

    private void HandleMovmementInput()
    {
        //used GetAxisRaw instead of GetAxis to make inputs feel more responsive
        currentInput = new Vector2(walkSpeed * Input.GetAxisRaw("Vertical"), walkSpeed * Input.GetAxisRaw("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -lowerLookLimit, upperLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxisRaw("Mouse X") * lookSpeedX, 0);
    }
    
    private void ApplyFinalMovements()
    {
        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        //some conditions will prevent the player from moving
        if (canMove)
        {
            HandleMovmementInput();
            HandleMouseLook();
            ApplyFinalMovements();
        }
    }
}
