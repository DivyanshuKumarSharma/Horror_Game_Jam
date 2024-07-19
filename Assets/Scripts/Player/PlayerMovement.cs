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

    [Header("Animator")]
    [SerializeField] private Animator playerAnim;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isStrafing = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isHeavyAttacking = false;

    [HideInInspector]
    public bool canMove = true;

    private float gravity = 20.0f;
    [SerializeField] private float jumpForce = 10;
    private float verticalVelocity = 0;

    [Header("Item")]
    public Inventory inventory;
    public Transform handTransform;
    private GameObject currentItemInHand;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (playerAnim == null)
        {
            playerAnim = GetComponent<Animator>();
        }

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
        HandleAttacks();
        HandleItemPickup();
        HandleItemSelection();
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

        isStrafing = Mathf.Abs(Input.GetAxis("Horizontal")) > 0 && Mathf.Abs(Input.GetAxis("Vertical")) == 0;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        isMoving = moveDirection.magnitude > 0 && !isRunning && !isStrafing && !isRunning;

        isJumping = characterController.isGrounded && Input.GetKey(KeyCode.Space);

        if (isJumping)
        {
            verticalVelocity += jumpForce;
            playerAnim.SetTrigger("jump");
            characterController.Move(moveDirection * Time.deltaTime);
        }
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

    private void HandleAttacks()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && !isHeavyAttacking)
        {
            // Determine attack type based on input (for example, hold Shift for heavy attack)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(HeavyAttack());
            }
            else
            {
                StartCoroutine(NormalAttack());
            }
        }
    }

    private IEnumerator NormalAttack()
    {
        isAttacking = true;
        playerAnim.SetTrigger("normalAttack");

        // Example attack animation coroutine
        yield return new WaitForSeconds(0.5f); // Adjust timing based on your animation

        isAttacking = false;
    }

    private IEnumerator HeavyAttack()
    {
        isHeavyAttacking = true;
        playerAnim.SetTrigger("heavyAttack");

        // Example heavy attack animation coroutine
        yield return new WaitForSeconds(1.0f); // Adjust timing based on your animation

        isHeavyAttacking = false;
    }

    private void AdjustArmPositions()
    {
        rightArm.position = playerCamera.transform.position + playerCamera.transform.TransformDirection(rightArmOffset);
        leftArm.position = playerCamera.transform.position + playerCamera.transform.TransformDirection(leftArmOffset);
    }

    private void HandleItemPickup()
    {
        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, 2f))
            if (inventory.AddItem(hit.collider.GetComponent<Item>())) Debug.Log($"Picked up item: {hit.collider.GetComponent<Item>().itemName}");
            else Debug.Log("Inventory is full.");
    }


    private void HandleItemSelection()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipItem(i);
            }
        }
    }

    private void EquipItem(int index)
    {
        Item item = inventory.GetItem(index);
        if (item == null)
        {
            Debug.LogWarning($"No item found at index: {index}");
            return;
        }

        // Destroy previous item in hand if exists
        if (currentItemInHand != null)
        {
            Destroy(currentItemInHand);
        }

        // Instantiate new item in hand
        currentItemInHand = Instantiate(item.gameObject, handTransform);
        currentItemInHand.transform.localPosition = Vector3.zero;
        currentItemInHand.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        currentItemInHand.SetActive(true);

        Debug.Log($"Equipped item: {item.itemName}");
    }



}
