using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ui;

    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private float playerAcceleration;
    [SerializeField] private float maxPlayerSpeed;
    [SerializeField] private float dragForce;
    [SerializeField] private float defaultDrag;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    [SerializeField] private float checkRadius;
    private Vector2 playerMoveVector;
    private bool isPlayerMoving;
    [SerializeField] private float maxRotation;
    private bool playerIsRight;
    private Animator animator;

    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerIsRight = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Input System callbacks
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            HandleJump();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        HandleMoveInput(context);
    }

    private void FixedUpdate()
    {
        // Check if grounded
        CheckGrounded();

        // Handle player movement
        HandlePlayerMovement();

        // Limit player rotation
        LimitPlayerRotation();
        
    }

    // Custom methods for better readability

    

    private void HandleJump()
    {
        rb.drag = defaultDrag;
        rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        float currentZRotation = transform.eulerAngles.z;

        if (context.started)
        {
            playerMoveVector = context.ReadValue<Vector2>();
            if (playerMoveVector.x > 0 && !playerIsRight)
            {
                transform.eulerAngles = new Vector3(0, 0, currentZRotation);
                playerIsRight = true;
            }
            else if (playerMoveVector.x < 0 && playerIsRight)
            {
                transform.eulerAngles = new Vector3(0, -180, currentZRotation);
                playerIsRight = false;
            }

            isPlayerMoving = true;
            rb.drag = defaultDrag;
        }
        else if (context.canceled)
        {
            playerMoveVector = context.ReadValue<Vector2>();
            rb.drag = dragForce;
            isPlayerMoving = false;
        }

        animator.SetBool("isMoving", isPlayerMoving);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, checkRadius, whatIsGround);
        animator.SetBool("isFalling", !isGrounded);
    }

    private void HandlePlayerMovement()
    {
        rb.AddForce(new Vector2(playerMoveVector.x * playerAcceleration, 0f));
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxPlayerSpeed);

        // Adds drag to the player when not pressing move keys and on the ground
        if (rb.velocity.x == 0f || !isGrounded)
        {
            rb.drag = defaultDrag;
        }
        else if (!isPlayerMoving && isGrounded)
        {
            rb.drag = dragForce;
        }
        else if (!isPlayerMoving && !isGrounded)
        {
            rb.drag = defaultDrag;
        }
    }

    private void LimitPlayerRotation()
    {
        var eulerAngles = transform.eulerAngles;
        float currentZRotation = eulerAngles.z;
        float clampedZRotation = Mathf.Clamp(currentZRotation, -maxRotation, maxRotation);
        eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, clampedZRotation);
    }

    public void GetCameraValues(out Rigidbody rbData, out float playerSpeedData)
    {
        rbData = rb;
        playerSpeedData = maxPlayerSpeed;
    }
}

