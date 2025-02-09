using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float proneSpeed = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float smoothing = 10f;
    [SerializeField] private float deceleration = 5f;

    [Header("Height Settings")]
    [SerializeField] private float standHeight = 1.6f;
    [SerializeField] private float crouchHeight = 0.6f;
    [SerializeField] private float proneHeight = 0.1f;

    [Header("Interaction Settings")]
    [SerializeField] private float pickupRange = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    private Camera playerCamera;

    private Vector3 moveDirection;
    private MovementState currentState = MovementState.Standing;
    private bool isRunning;
    private float verticalVelocity;
    private Vector3 velocitySmoothDamp;

    private enum MovementState
    {
        Standing,
        Crouching,
        Prone
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        playerCamera = GetComponentInChildren<Camera>();

        if (!characterController || !capsuleCollider || !animator || !playerCamera)
        {
            Debug.LogError("Missing required components: CharacterController, CapsuleCollider, Animator, or Camera.");
        }
    }

    private void Update()
    {
        ProcessInput();
        MovePlayer();
        AdjustColliderAndCamera();
        HandleItemPickup();
        UpdateAnimator();
    }

    private void ProcessInput()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift) && currentState == MovementState.Standing;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentState = MovementState.Crouching;
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            currentState = MovementState.Prone;
        }
        else
        {
            currentState = MovementState.Standing;
        }
    }

    private void MovePlayer()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputDirection.magnitude > 0)
        {
            Vector3 targetDirection = transform.TransformDirection(inputDirection.normalized) * GetCurrentSpeed();
            moveDirection = targetDirection;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        HandleJumpAndGravity();
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleJumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump") && currentState == MovementState.Standing)
            {
                verticalVelocity = jumpForce;
            }
            else
            {
                verticalVelocity = -2f;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
    }

    private float GetCurrentSpeed()
    {
        switch (currentState)
        {
            case MovementState.Crouching:
                return crouchSpeed;
            case MovementState.Prone:
                return proneSpeed;
            case MovementState.Standing:
            default:
                return isRunning ? runSpeed : walkSpeed;
        }
    }

    private void AdjustColliderAndCamera()
    {
        float targetHeight = currentState switch
        {
            MovementState.Crouching => crouchHeight,
            MovementState.Prone => proneHeight,
            _ => standHeight
        };

        AdjustColliderHeight(targetHeight);
        AdjustCameraHeight(targetHeight);
    }

    private void AdjustColliderHeight(float targetHeight)
    {
        capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, targetHeight, Time.deltaTime * 10f);
        capsuleCollider.center = new Vector3(0, capsuleCollider.height / 2, 0);
    }

    private void AdjustCameraHeight(float targetHeight)
    {
        Vector3 cameraPosition = playerCamera.transform.localPosition;
        cameraPosition.y = Mathf.Lerp(cameraPosition.y, targetHeight - 0.2f, Time.deltaTime * 10f);
        playerCamera.transform.localPosition = cameraPosition;
    }

    private void HandleItemPickup()
    {
        if (Input.GetKeyDown(interactKey))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
            {
                var pickupItem = hit.collider.GetComponent<PickupItem>();
                pickupItem?.Pickup();
            }
        }
    }

    private void UpdateAnimator()
    {
        Vector3 horizontalMove = new Vector3(moveDirection.x, 0, moveDirection.z);
        float speed = horizontalMove.magnitude;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning);
    }
}
