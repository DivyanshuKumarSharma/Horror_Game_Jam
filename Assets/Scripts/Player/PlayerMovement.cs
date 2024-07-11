using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookLimit = 45.0f;
    [SerializeField] private Camera playerCamera;

    [Header("Player Arms")]
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform baseballBat;

    [Header("Arm Offset Settings")]
    [SerializeField] private Vector3 rightArmOffset;
    [SerializeField] private Vector3 leftArmOffset;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private bool isRunning = false;

    [HideInInspector]
    public bool canMove = true;

    private float gravity = 20.0f;
    private float verticalVelocity = 0;
    private bool isAttacking = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Make sure the baseball bat is a child of the right arm
        baseballBat.SetParent(rightArm);
    }

    void Update()
    {
        HandleMovement();
        ApplyGravity();
        MoveCharacter();
        HandleCameraRotation();
        HandleAttack();
    }

    void LateUpdate()
    {
        AdjustArmPositions();
    }

    private void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0; // Reset vertical velocity when grounded
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // Apply gravity
        }

        moveDirection.y = verticalVelocity;
    }

    private void MoveCharacter()
    {
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookLimit, lookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Debug.Log("Left mouse button clicked, initiating attack.");
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        Debug.Log("Attack coroutine started.");

        // Initial positions and rotations
        Vector3 initialArmPosition = rightArm.localPosition;
        Quaternion initialArmRotation = rightArm.localRotation;

        Debug.Log("Initial arm position: " + initialArmPosition);
        Debug.Log("Initial arm rotation: " + initialArmRotation);

        // Prepare the arm for the attack (move back)
        Vector3 attackPositionBack = initialArmPosition + new Vector3(-0.5f, 0, 0); // Adjust as needed
        Quaternion attackRotationBack = initialArmRotation * Quaternion.Euler(0, 0, -30); // Adjust as needed

        // Move back
        float elapsedTime = 0;
        float attackDuration = 0.1f; // Adjust as needed
        while (elapsedTime < attackDuration)
        {
            rightArm.localPosition = Vector3.Lerp(initialArmPosition, attackPositionBack, elapsedTime / attackDuration);
            rightArm.localRotation = Quaternion.Slerp(initialArmRotation, attackRotationBack, elapsedTime / attackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Moved back to attack position.");

        // Move forward to hit
        Vector3 attackPositionForward = initialArmPosition + new Vector3(0.5f, 0, 0); // Adjust as needed
        Quaternion attackRotationForward = initialArmRotation * Quaternion.Euler(0, 0, 30); // Adjust as needed

        elapsedTime = 0;
        attackDuration = 0.2f; // Adjust as needed
        while (elapsedTime < attackDuration)
        {
            rightArm.localPosition = Vector3.Lerp(attackPositionBack, attackPositionForward, elapsedTime / attackDuration);
            rightArm.localRotation = Quaternion.Slerp(attackRotationBack, attackRotationForward, elapsedTime / attackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Moved forward to hit position.");

        // Reset to initial position and rotation
        elapsedTime = 0;
        attackDuration = 0.1f; // Adjust as needed
        while (elapsedTime < attackDuration)
        {
            rightArm.localPosition = Vector3.Lerp(attackPositionForward, initialArmPosition, elapsedTime / attackDuration);
            rightArm.localRotation = Quaternion.Slerp(attackRotationForward, initialArmRotation, elapsedTime / attackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rightArm.localPosition = initialArmPosition;
        rightArm.localRotation = initialArmRotation;

        isAttacking = false;
        Debug.Log("Attack coroutine finished.");
    }

    private void AdjustArmPositions()
    {
        rightArm.position = playerCamera.transform.position + playerCamera.transform.TransformDirection(rightArmOffset);
        leftArm.position = playerCamera.transform.position + playerCamera.transform.TransformDirection(leftArmOffset);
    }
}
