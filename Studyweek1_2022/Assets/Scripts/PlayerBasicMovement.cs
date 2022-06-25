using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicMovement : MonoBehaviour
{
    #region Variables

    private AudioSource _playerJumpSound;
    private AudioSource _playerWalkSound;
    private PlayerMovement_Controls _playerControls;
    private InputAction _groundMovement;
    private Rigidbody2D _playerRb;
    [SerializeField] private float jumpForce = 40;
    private float normalGravityScale = 1.75f;
    [SerializeField] private float fallingGravityScale = 0.4f;
    private float _moveInput;
    [SerializeField] private float runSpeed = 15f;
    private float runMaxSpeed = 40f;
    [SerializeField] private float jumpHorizontalSpeed = 9f;
    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float deceleration = 7f;
    [SerializeField] private bool groundCheck = false;
    private bool _secondJump = false;
    [SerializeField] private float jumpCutMultiplier = 0.2f;
    //[SerializeField] private bool EnableDoubleJump = false;
    public SettingsData gameSettings;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Transform groundCheckCollider2;
    [SerializeField] Transform groundCheckCollider1;
    [SerializeField] private float secondJumpForce = 60;

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
        _playerJumpSound = GetComponent<AudioSource>();
        _playerWalkSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //_playerWalkSound.Play();
        _playerRb = GetComponent<Rigidbody2D>();
        _groundMovement.Enable();
        _playerControls.Player.Jump.performed += PlayerJump;
        _playerControls.Player.Jump.canceled += PlayerJump;
        _playerControls.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        _groundMovement.Disable();
    }

    private void FixedUpdate()
    {
        GroundCheck();

        #region MovementSpeed_Berechnung

        if (_groundMovement.ReadValue<float>() < 0)
        {
            transform.localScale = new Vector2((Mathf.Sign(_playerRb.velocity.x)), transform.localScale.y);
        }

        if (_groundMovement.ReadValue<float>() > 0)
        {
            transform.localScale = new Vector2((Mathf.Sign(_playerRb.velocity.x)), transform.localScale.y);
        }


        _moveInput = _groundMovement.ReadValue<float>();

        float targetSpeed = _moveInput * runSpeed;
        float speedDiff = targetSpeed - _playerRb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.87f) * Mathf.Sign(speedDiff);

        _playerRb.AddForce(Vector2.right * movement);

        #endregion

        AnimatorStates();

        #region GravityFallAdjustment

        //Gravity scales up to make falling more "realistic".
        //Groundcheck gets updated
        if (_playerRb.velocity.y < 0.1f || _playerRb.velocity.y != 0)
        {
            _playerRb.gravityScale += fallingGravityScale;
        }

        #endregion
    }

    #region JumpMethode

    private void PlayerJump(InputAction.CallbackContext obj)
    {
        //Groundcheck gets called to prevent infinite jumps.       
        if (groundCheck && obj.performed)
        {
            _playerJumpSound.Play();
            groundCheck = false;
            runSpeed = jumpHorizontalSpeed;
            _playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            StartCoroutine(StartCooldown());
        }


        //The longer the jump button is pressed, the higher the jump.
        if (obj.canceled && _playerRb.velocity.y > 0)
        {
            _playerRb.AddForce(Vector2.down * _playerRb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }

        //Optional DoubleJump (WIP)
        if (gameSettings.enableDoubleJump && _secondJump && !groundCheck && obj.performed)
        {
            _playerRb.AddForce(new Vector2(0, secondJumpForce), ForceMode2D.Impulse);
            _playerJumpSound.Play();
            _secondJump = false;
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
            _playerRb.gravityScale = normalGravityScale;
            runSpeed = runMaxSpeed;
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

    public void AnimatorStates()
    {
        if (!groundCheck)
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsJumping", true);
        }
        else if (groundCheck && _playerRb.velocity.x == 0 ||
                 _playerRb.velocity.x <= 2.04f && _playerRb.velocity.x >= -2.04f)
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsJumping", false);
        }
        else if (groundCheck && _playerRb.velocity.x != Mathf.Epsilon)
        {
            _animator.SetBool("IsWalking", true);
            _animator.SetBool("IsJumping", false);
        }
    }  
}