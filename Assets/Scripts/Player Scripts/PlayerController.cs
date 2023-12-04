using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Animator animator;
    public PlayerStats playerStats;
    private PlayerStatsManager playerStatsManager;
    public ParticleSystem HealthParticles;
    public ParticleSystem SpeedParticles;
    private UIManager uiManager;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float playerAcceleration;
    private float maxPlayerSpeed;
    [SerializeField] private float dragForce;
    [SerializeField] private float defaultDrag;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float checkRadius;
    private Vector2 playerMoveVector;
    private bool isPlayerMoving;
    [SerializeField] private float maxRotation;
    private bool isPlayerRight;
    private bool isGrounded;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletForce;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats.playerSpeed = 20f;
        maxPlayerSpeed = playerStats.playerSpeed;
        playerStatsManager = GetComponent<PlayerStatsManager>();
        animator = GetComponent<Animator>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && playerStatsManager.playerStats.Ammo > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Instantiate a bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 10));

        // Calculate the direction towards the mouse in world space
        Vector3 shootDirection = (mousePosition - firePoint.position).normalized;

        // Set the bullet's velocity to move towards the mouse
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = new Vector3(shootDirection.x * bulletForce, shootDirection.y * bulletForce, 0);
        }
        playerStatsManager.Shoot();
    }

    // Input System callbacks
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.drag = defaultDrag;
            float scaledJumpForce = jumpForce * Mathf.Abs(rb.velocity.x) / maxPlayerSpeed;
            float clampedJumpForce = Mathf.Clamp(scaledJumpForce, playerStats.JumpHeight, Mathf.Infinity);
            rb.AddForce(Vector2.up * clampedJumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    public void OnUseHealthPotion(InputAction.CallbackContext context)
    {
        playerStatsManager.Invoke("UseHealthPotion", .5f);
        HealthParticles.Play();
    }

    public void OnUseSpeedPotion(InputAction.CallbackContext context)
    {
        playerStatsManager.Invoke("UseSpeedBoost", .5f);
        SpeedParticles.Play();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float currentZRotation = transform.eulerAngles.z;
        playerMoveVector = context.ReadValue<Vector2>();
        if (context.started)
        {
            isPlayerMoving = true;
            rb.drag = defaultDrag;
        }
        else if (context.canceled)
        {
            rb.drag = dragForce;
            isPlayerMoving = false;
        }

        animator.SetBool("isMoving", isPlayerMoving);
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

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(groundChecker.position, Vector3.down, checkRadius, whatIsGround);
        animator.SetBool("isFalling", !isGrounded);
    }

    private void HandlePlayerMovement()
    {
        float horizontalForce = playerMoveVector.x * playerAcceleration;

        // Apply horizontal force
        rb.AddForce(new Vector2(horizontalForce, 0f));

        // Clamp player velocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, playerStats.playerSpeed);

        // Handle sliding
        if (!isPlayerMoving && isGrounded)
        {
            ApplySlideForce();
        }
        else
        {
            rb.drag = defaultDrag;
        }
        
        UpdatePlayerRotation();
    }

    private void UpdatePlayerRotation()
    {
        // Get the mouse position
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Convert the mouse position to a world point
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));

        // Flip the player based on the mouse position
        if (worldMousePosition.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Facing right
            isPlayerRight = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Facing left
            isPlayerRight = false;
        }
    }

    private void ApplySlideForce()
    {
        // Calculate the opposite force to simulate sliding
        float slideForce = -Mathf.Sign(rb.velocity.x) * dragForce;

        // Apply the slide force
        rb.AddForce(new Vector2(slideForce, 0f) * Time.deltaTime);

        // Optionally, you can clamp the slide force to prevent excessive sliding
        float maxSlideForce = 10f;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSlideForce, maxSlideForce), rb.velocity.y);
    }

    private void LimitPlayerRotation()
    {
        var eulerAngles = transform.eulerAngles;
        float currentZRotation = eulerAngles.z;
        float clampedZRotation = Mathf.Clamp(currentZRotation, -maxRotation, maxRotation);
        eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, clampedZRotation);
        transform.eulerAngles = eulerAngles; // Apply the clamped rotation back to the transform
    }

    public void GetCameraValues(out Rigidbody rbData, out float playerSpeedData)
    {
        rbData = rb;
        playerSpeedData = maxPlayerSpeed;
    }
}

