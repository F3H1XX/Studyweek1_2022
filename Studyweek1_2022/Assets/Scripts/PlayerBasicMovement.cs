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
    private float _moveInput;
    [SerializeField] private float runSpeed = 15f;
    private float runMaxSpeed = 40f;
    [SerializeField] private float jumpHorizontalSpeed = 9f;
    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float deceleration = 7f;
    [SerializeField] private float defaultGravity = 10f;
    [SerializeField] private bool groundCheck = false;
    [SerializeField] private bool clingCheck = false;
    [SerializeField] private bool coyoteTimeRunning = false;
    [SerializeField]private bool bufferActive = false;
    [SerializeField] private bool saveJump = false;
    private bool _secondJump = false;
    [SerializeField] private float jumpCutMultiplier = 0.2f;
    [SerializeField] private float coyoteTime = 0.5f;
    private float _coyoteTimeCounter;
    public SettingsData gameSettings;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] Transform groundCheckCollider1;
    [SerializeField] private Transform WallClingCheck;
    [SerializeField] private Transform JumpBufferCheck;
    [SerializeField] private float secondJumpForce = 0;
    Collider2D[] walls;
    

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
            transform.localScale = new Vector2(-1f, 1);
        }

        if (_groundMovement.ReadValue<float>() > 0)
        {
            transform.localScale = Vector2.one;
        }

        _moveInput = _groundMovement.ReadValue<float>();
       

        float targetSpeed = _moveInput * runSpeed;
        float speedDiff = targetSpeed - _playerRb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.87f) * Mathf.Sign(speedDiff);

        _playerRb.AddForce(Vector2.right * movement);

        #endregion

        Collider2D[] jumpBufferColliders = new Collider2D[20];
        jumpBufferColliders = Physics2D.OverlapCircleAll(JumpBufferCheck.position, 0.5f, groundLayer);

        if (jumpBufferColliders.Length != 0 && !groundCheck)
            bufferActive = true;
        if (jumpBufferColliders.Length == 0)
            bufferActive = false;



        AnimatorStates();

        #region WallCling

        walls = Physics2D.OverlapCircleAll(WallClingCheck.position, 0.1f, wallLayer);

        if (walls.Length != 0 && groundCheck == false)
        {
            if ((transform.localScale.x == 1f && _groundMovement.ReadValue<float>() != 0) || (transform.localScale.x == -1f && _groundMovement.ReadValue<float>() != 0))
            {
                clingCheck = true;
            }
            else
            {
                StartCoroutine(BoolSetCooldown(clingCheck));
            }
        }
        else
        {
            StartCoroutine(BoolSetCooldown(clingCheck));
        }

        if (clingCheck)
        {
            _playerRb.gravityScale = 0;
            _playerRb.velocity = Vector2.zero;
        }
        else if (!clingCheck)
        {
            _playerRb.gravityScale = defaultGravity;
        }

        if(groundCheck && saveJump)
        {
            Debug.Log("Bung");
            _playerRb.velocity = Vector2.zero;
            _playerRb.AddForce(new Vector2(0, jumpForce ), ForceMode2D.Impulse);
            runSpeed = jumpHorizontalSpeed;
            groundCheck = false;
            saveJump = false;
            _coyoteTimeCounter = 0;
            _playerJumpSound.Play();

            StartCoroutine(DoubleJumpCooldown());
        }
        

       
        
        #endregion
    }

    #region JumpMethode

    private void PlayerJump(InputAction.CallbackContext obj)
    {


        if (bufferActive && !groundCheck && obj.performed)
        {
            saveJump = true;
        }
        //Ground-check gets called to prevent infinite jumps.       
        if (groundCheck && obj.performed)
        {
            Debug.Log("Beng");
            _playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            runSpeed = jumpHorizontalSpeed;
            groundCheck = false;
            saveJump = false;
            _coyoteTimeCounter = 0;
            _playerJumpSound.Play();

            StartCoroutine(DoubleJumpCooldown());
        }  

        if(coyoteTimeRunning)
        {
            _secondJump = true;
        }
       
        
        //Optional DoubleJump (WIP)
        if (gameSettings.enableDoubleJump && _secondJump && !groundCheck && obj.performed && !clingCheck)
        {

            
            Debug.Log("boing");
           
            
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, 0);
                _playerRb.AddForce(new Vector2(0, secondJumpForce), ForceMode2D.Impulse);
                _secondJump = false;
                _playerJumpSound.Play();             
            
        }

        //The longer the jump button is pressed, the higher the jump.
        if (obj.canceled && _playerRb.velocity.y > 0)
        {
            _playerRb.AddForce(Vector2.down * _playerRb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }


        if (clingCheck && obj.performed && walls.Length == 0)
        {
            _playerRb.AddForce(new Vector2(0, secondJumpForce), ForceMode2D.Impulse);
            _playerJumpSound.Play();
            clingCheck = false;
            runSpeed = runMaxSpeed;
            StartCoroutine(BoolSetCooldown(clingCheck));
        }
        groundCheck = false;
    }

    #endregion

    #region GroundCheck

    public void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider1.position, 0.3f, groundLayer);

        //Overlaps check for groundLayer in radius, to see if the player touches the ground
        if (colliders.Length > 0)
        {          
            runSpeed = runMaxSpeed;
            _secondJump = false;
            groundCheck = true;
            

          
            _coyoteTimeCounter = coyoteTime;
            
        }
        //coyoteTime enables the player to jump slightly after leaving the ground.
        else if (colliders.Length == 0)
        {
            groundCheck = false;
            _coyoteTimeCounter -= Time.deltaTime;
            if (_coyoteTimeCounter > 0f && _coyoteTimeCounter < 0.5f)
            {
                coyoteTimeRunning = true;
            }
            if (_coyoteTimeCounter <= 0f)
            {
               
                coyoteTimeRunning = false;
            }
            if(_secondJump)
            {
                _coyoteTimeCounter = 0f;
            }
        }
    }

    #endregion

    //Cooldown to prevent jump and second jump to trigger simultaneously
    public IEnumerator DoubleJumpCooldown()
    {
        yield return new WaitForSeconds(0.05f);
        _secondJump = true;

    }

    public IEnumerator BoolSetCooldown(bool value)
    {
        yield return new WaitForSeconds(0.2f);
        value = false;
    }

    private void AnimatorStates()
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