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
    [SerializeField] private float playerSpeed;
    [SerializeField] private InputActionAsset inputAction;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputAction = GetComponent<InputActionAsset>();
        inputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnJump()
    {
        rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
    }

    public void OnMove(InputValue value)
    {
        playerMoveVector = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(playerMoveVector.x * playerSpeed,rb.velocity.y);
        rb.AddForce(new Vector2(playerMoveVector.x * playerSpeed, 0f));
        
    }
}
