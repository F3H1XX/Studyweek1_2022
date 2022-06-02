using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicMovement : MonoBehaviour
{
    #region Variables
    private PlayerMovement_Controls playerControls;
    private InputAction groundMovement;
    private Rigidbody2D playerRB;
    [SerializeField] private float jumpForce = 40;
    private float normalGravityScale = 1.75f;
    [SerializeField] private float fallingGravityScale = 0.4f;
    private float moveInput;
    [SerializeField] private float runSpeed = 15f;
    private float runMaxSpeed = 40f;
    [SerializeField] private float jumpHorizontalSpeed = 9f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float decceleration = 3f;
    [SerializeField] private bool groundCheck = false;
    private bool secondJump = false;
    [SerializeField] private float jumpCutMultiplier = 0.2f;
    [SerializeField] private bool doubleJumpEnabled = false;
    [SerializeField] private float secondJumpForce = 80;
    [SerializeField] Transform groundCheckCollider1;
    [SerializeField] Transform groundCheckCollider2;
    [SerializeField] private LayerMask groundLayer;


    #endregion

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
        playerControls.Player.Jump.canceled += playerJump;
        playerControls.Player.Jump.Enable();
    }
    private void OnDisable()
    {
        groundMovement.Disable();
    }
    private void FixedUpdate()
    {
        GroundCheck();
        #region MovementSpeed_Berechnung

        /*Calculates velocity of player until max speed is reached.
           Movement is more fluent */

        moveInput = groundMovement.ReadValue<float>();

        float targetSpeed = moveInput * runSpeed;
        float speedDiff = targetSpeed - playerRB.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.87f) * Mathf.Sign(speedDiff);

        playerRB.AddForce(Vector2.right * movement);
        #endregion


        #region GravityFallAdjustment

        //Gravity scales up to make falling more "realistic".
        //Groundcheck gets updated
        if (playerRB.velocity.y < 0.1f || playerRB.velocity.y != 0)
        {
            playerRB.gravityScale += fallingGravityScale;
        }

        #endregion
    }

    #region JumpMethode
    private void playerJump(InputAction.CallbackContext obj)
    {
        //Groundcheck gets called to prevent infinite jumps.       
        if (groundCheck && obj.performed)
        {

            groundCheck = false;
            runSpeed = jumpHorizontalSpeed;
            playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            StartCoroutine(StartCooldown());
        }

        //The longer the jump button is pressed, the higher the jump.
        if (obj.canceled && playerRB.velocity.y > 0)
        {
            playerRB.AddForce(Vector2.down * playerRB.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }

        //Optional DoubleJump (WIP)
        if (doubleJumpEnabled && secondJump && !groundCheck && obj.performed)
        {
            Debug.Log("I doublejumped");
            playerRB.AddForce(new Vector2(0, secondJumpForce), ForceMode2D.Impulse);
            secondJump = false;
        }
        groundCheck = false;
    }
    #endregion

    #region GroundCheck
    public void GroundCheck()
    {
        //One Overlap for each leg, so the player doesn't get stuck on ledges
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider1.position, 0.3f, groundLayer);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(groundCheckCollider2.position, 0.3f, groundLayer);

        groundCheck = false;
        //Overlaps check for groundLayer in radius, to see if the palyer touches the ground
        if (colliders.Length > 0 || colliders2.Length > 0)
        {
            groundCheck = true;
            playerRB.gravityScale = normalGravityScale;
            runSpeed = runMaxSpeed;
            secondJump = false;
        }
    }
    #endregion
    //Cooldown to prevent jump and second jump to trigger simultaneously
    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        secondJump = true;
    }
}