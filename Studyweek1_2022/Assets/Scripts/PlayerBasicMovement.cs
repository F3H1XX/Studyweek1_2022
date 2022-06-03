using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicMovement : MonoBehaviour
{
    #region Variables
    private PlayerMovement_Controls _playerControls;
    private InputAction _groundMovement;
    private Rigidbody2D _playerRb;
    [SerializeField] private float JumpForce = 40;
    private float normalGravityScale = 1.75f;
    [SerializeField] private float FallingGravityScale = 0.4f;
    private float _moveInput;
    [SerializeField] private float RunSpeed = 15f;
    private float runMaxSpeed = 40f;
    [SerializeField] private float JumpHorizontalSpeed = 9f;
    [SerializeField] private float Acceleration = 2f;
    [SerializeField] private float decceleration = 3f;
    [SerializeField] private bool GroundCheck = false;
    private bool _secondJump = false;
    [SerializeField] private float JumpCutMultiplier = 0.2f;
    //[SerializeField] private bool EnableDoubleJump = false;
    [SerializeField] private float SecondJumpForce = 80;
    [SerializeField] Transform GroundCheckCollider1;
    [SerializeField] Transform GroundCheckCollider2;
    [SerializeField] private LayerMask GroundLayer;
    public SettingsData GameSettings;

    #endregion

    private Animator _animator;
    //private static SettingsData GameSettings = SettingsData.CreateInstance < "SettingsData" >;
    private void Awake()
    {
        
        //EnableDoubleJump = GameSettings.EnableDoubleJump; 
        _playerControls = new PlayerMovement_Controls();
        
        _groundMovement = _playerControls.Player.GroundMovement;

        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _animator.enabled = true;
        _animator.SetBool("IsWalking", true);
    }

    private void OnEnable()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _groundMovement.Enable();
        _playerControls.Player.Jump.performed += playerJump;
        _playerControls.Player.Jump.canceled += playerJump;
        _playerControls.Player.Jump.Enable();
    }
    private void OnDisable()
    {
        _groundMovement.Disable();
    }
    private void FixedUpdate()
    {
        groundCheck();
        #region MovementSpeed_Berechnung

        /*Calculates velocity of player until max speed is reached.
           Movement is more fluent */

        _moveInput = _groundMovement.ReadValue<float>();

        float targetSpeed = _moveInput * RunSpeed;
        float speedDiff = targetSpeed - _playerRb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.87f) * Mathf.Sign(speedDiff);

        _playerRb.AddForce(Vector2.right * movement);
        #endregion


        #region GravityFallAdjustment

        //Gravity scales up to make falling more "realistic".
        //Groundcheck gets updated
        if (_playerRb.velocity.y < 0.1f || _playerRb.velocity.y != 0)
        {
            _playerRb.gravityScale += FallingGravityScale;
        }

        #endregion
    }

    #region JumpMethode
    private void playerJump(InputAction.CallbackContext obj)
    {
        //Groundcheck gets called to prevent infinite jumps.       
        if (GroundCheck && obj.performed)
        {

            GroundCheck = false;
            RunSpeed = JumpHorizontalSpeed;
            _playerRb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            StartCoroutine(StartCooldown());
        }
        

        //The longer the jump button is pressed, the higher the jump.
        if (obj.canceled && _playerRb.velocity.y > 0)
        {
            _playerRb.AddForce(Vector2.down * _playerRb.velocity.y * (1 - JumpCutMultiplier), ForceMode2D.Impulse);
        }
        
        //Optional DoubleJump (WIP)
        if (GameSettings.EnableDoubleJump && _secondJump && !GroundCheck && obj.performed)
        {
            Debug.Log("I doublejumped");
            _playerRb.AddForce(new Vector2(0, SecondJumpForce), ForceMode2D.Impulse);
            _secondJump = false;
        }
        GroundCheck = false;
    }
    #endregion

    #region GroundCheck
    public void groundCheck()
    {
        //One Overlap for each leg, so the player doesn't get stuck on ledges
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckCollider1.position, 0.3f, GroundLayer);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(GroundCheckCollider2.position, 0.3f, GroundLayer);

        GroundCheck = false;
        //Overlaps check for groundLayer in radius, to see if the palyer touches the ground
        if (colliders.Length > 0 || colliders2.Length > 0)
        {
            GroundCheck = true;
            _playerRb.gravityScale = normalGravityScale;
            RunSpeed = runMaxSpeed;
            _secondJump = false;
        }
    }
    #endregion
    //Cooldown to prevent jump and second jump to trigger simultaneously
    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        _secondJump = true;
    }
}