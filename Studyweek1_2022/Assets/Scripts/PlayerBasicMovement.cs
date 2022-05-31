using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicMovement : MonoBehaviour
{
    
    private PlayerMovement_Controls playerControls;
    private InputAction groundMovement;
    private Rigidbody2D playerRB;
    [SerializeField] private float jumpForce;
    private float normalGravityScale = 1.5f;
    private float fallingGravityScale = 1.1f;
    private float moveInput;
    public float runMaxSpeed = 3;
    private float velocityX;

    private void Awake()
    {
        playerControls = new PlayerMovement_Controls();
        playerRB = GetComponent<Rigidbody2D>();
        groundMovement = playerControls.Player.GroundMovement;
    }
    private void OnEnable()
    {            
        groundMovement.Enable();
        playerControls.Player.Jump.performed += playerJump;
        playerControls.Player.Jump.Enable();       
    }
    private void OnDisable()
    {
        groundMovement.Disable();
    }
    private void FixedUpdate()
    {
        Debug.Log(moveInput);
        velocityX = transform.position.x + runMaxSpeed * moveInput * Time.deltaTime;
        moveInput = groundMovement.ReadValue<float>();
        if(moveInput > 0)
        {
            transform.position = new Vector2(velocityX, transform.position.y);
        }
        else if (moveInput < 0)
        {
            
            transform.position = new Vector2(velocityX, transform.position.y);
        }
        if (playerRB.velocity.y >= 0)
        {
            playerRB.gravityScale = normalGravityScale;
        }
        else if (playerRB.velocity.y < 0)
        {
            playerRB.gravityScale *= fallingGravityScale;           
        }
    } 
    private void playerJump(InputAction.CallbackContext obj)
    {
        Debug.Log("I jumped!");       
        playerRB.AddForce( new Vector2(0, jumpForce), ForceMode2D.Impulse);
        
    }


}
