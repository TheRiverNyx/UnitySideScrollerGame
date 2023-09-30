using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private Rigidbody2D rb;
    private Vector2 playerMoveVector;
    [SerializeField] private float jumpForce;
    [SerializeField] private float playerAcceleration;
    [SerializeField] private float maxPlayerSpeed;
    [SerializeField] private float dragForce;
    [SerializeField] private float defaultDrag;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask whatIsGround;
    public bool isGrounded;
    [SerializeField] private float checkRadius;
    private bool isPlayerMoving;
    public float maxRotation;
    private bool playerIsRight;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerIsRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.gameObject.transform.position = new Vector3(rb.position.x,rb.position.y,playerCamera.gameObject.transform.position.z);
        
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.drag = defaultDrag;
            rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerMoveVector = context.ReadValue<Vector2>();
            if (playerMoveVector.x > 0 && !playerIsRight)
            {
                transform.localScale = new Vector3(
                    1,
                    transform.localScale.y,
                    transform.localScale.z
                );
                playerIsRight = true;
                
            } else if(playerMoveVector.x<0 && playerIsRight) {
                transform.localScale = new Vector3(
                    -1,
                    transform.localScale.y,
                    transform.localScale.z
                    );
                playerIsRight = false;
            }
            
            isPlayerMoving = true;
            rb.drag = defaultDrag;
            
        }else if (context.canceled)
        {
            playerMoveVector = context.ReadValue<Vector2>();
            rb.drag = dragForce;
            isPlayerMoving = false;
        }
        animator.SetBool("isMoving", isPlayerMoving);
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, checkRadius, whatIsGround);
        animator.SetBool("isFalling",!isGrounded);
        rb.AddForce(new Vector2(playerMoveVector.x * playerAcceleration, 0f));
        rb.velocity=Vector2.ClampMagnitude(rb.velocity, maxPlayerSpeed);
        //adds drag to player when not player not pressing move keys and on the ground
        if (rb.velocity.x == 0f || !isGrounded)
        {
            rb.drag = defaultDrag;
        }else if (!isPlayerMoving && isGrounded)
        {
            rb.drag = dragForce;
        } else if (!isPlayerMoving && !isGrounded)
        {
            rb.drag = defaultDrag;
        }
        rb.rotation = Mathf.Clamp(rb.rotation, -maxRotation, maxRotation);
    }
}
