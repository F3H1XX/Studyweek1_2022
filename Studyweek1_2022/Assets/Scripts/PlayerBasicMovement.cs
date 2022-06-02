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
    private float normalGravityScale = 1.75f;
    [SerializeField] private float fallingGravityScale = 0.4f;
    private float moveInput;
    [SerializeField] private float runMaxSpeed = 15;
    [SerializeField] private float jumpHorizontalSpeed = 7;
    [SerializeField]private float acceleration = 2;
    [SerializeField]private float decceleration = 3;
    private bool groundCheck = false;
    private bool secondJump = false;
    [SerializeField] private bool doubleJumpEnabled = false;   

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
        Debug.Log(playerRB.gravityScale);
        moveInput = groundMovement.ReadValue<float>();

        float targetSpeed = moveInput * runMaxSpeed;
        float speedDiff =  targetSpeed - playerRB.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.87f) * Mathf.Sign(speedDiff);

        playerRB.AddForce(Vector2.right * movement);                
        
        
        
        if (playerRB.velocity.y < 0.1f || playerRB.velocity.y != 0)
        {
            playerRB.gravityScale += fallingGravityScale; 
            
            groundCheck = false;
        }
        if(playerRB.velocity.y == 0)
        {
            playerRB.gravityScale = normalGravityScale;
            runMaxSpeed = 15;
            groundCheck = true;           
        }
    } 
    private void playerJump(InputAction.CallbackContext obj)
    {
        Debug.Log(groundCheck);
        if(groundCheck)
        {
            groundCheck = false;
            runMaxSpeed = jumpHorizontalSpeed;          
            playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);            
            secondJump = true;
        }       
        
        if(doubleJumpEnabled && secondJump && !groundCheck)
        {                              
            playerRB.AddForce(new Vector2(0, jumpForce / 1.5f), ForceMode2D.Impulse);                    
        }
        groundCheck = false;
    }  
}
