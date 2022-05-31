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
    public float runMaxSpeed = 4;
    private float velocityX;
    private float acceleration = 2;
    private float decceleration = 3;

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
        moveInput = groundMovement.ReadValue<float>();
        float targetSpeed = moveInput * runMaxSpeed;
        float speedDiff =  targetSpeed - playerRB.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.87f) * Mathf.Sign(speedDiff);

        playerRB.AddForce(Vector2.right * movement);
        Debug.Log(movement);
        
        
        
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
        Debug.Log(moveInput);
    }


}
