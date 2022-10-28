using System.Collections;
using UnityEngine;

public class MovManager : MonoBehaviour
{
    
    private float _moveSpeed;
    [Header("Speed")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 12.0f;
    public float crouchSpeed = 3.5f;

    [Header("Physics")]
    public float groundDrag = 5.0f;

    [Header("Jump")] 
    public float airMultiplier = 0.4f;
    public float jumpForce = 6.0f;
    public float jumpCooldown = 0.25f;
    private bool _isReadyToJump;

    [Header("Binds")] 
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    
    private float _horizontalMov;
    private float _verticalMov;
    
    private float _crouchYScale = 0.5f;
    private float _startYScale;
    private bool _isCrouching;

    private Transform _orientation;
    private Vector3 _direction;
    private Rigidbody _rb;
    
    private float _height = 2.0f;
    private LayerMask _groundMask;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _groundMask = LayerMask.GetMask("Default");
        _orientation = GameObject.Find("Orientation").transform.GetComponent<Transform>();
    }

    private void Start() {
        _rb.freezeRotation = true;
        _moveSpeed = walkSpeed;
        _isReadyToJump = true;
        _startYScale = transform.localScale.y;
    }

    private void Update() {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, (_height * 0.5f) + 0.2f, _groundMask);
        
        InputHandler();
        VelocityHandler();
        Run();

        if (_isGrounded) {
            _rb.drag = groundDrag;
        } else {
            _rb.drag = 0;
        }
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        _direction = (_orientation.forward * _verticalMov) + (_orientation.right * _horizontalMov);

        if (_isGrounded) {
            _rb.AddForce(_direction.normalized * (_moveSpeed * 10.0f), ForceMode.Force);
        } else if (!_isGrounded) {
            _rb.AddForce(_direction.normalized * (_moveSpeed * 10.0f * airMultiplier), ForceMode.Force);
        }
    }
    
    private void Run() {
        if (_isGrounded && !_isCrouching && Input.GetKeyDown(runKey)) {
            _moveSpeed = runSpeed;
        } else if (_isGrounded) {
            _moveSpeed = walkSpeed;
        }
    }

    private void Jump() {
        Vector3 velocity = _rb.velocity;
        _rb.velocity = new Vector3(velocity.x, 0.0f, velocity.z);
        
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private IEnumerator JumpReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isReadyToJump = true;
    }

    private void BeginCrouch() {
        transform.localScale = new Vector3(transform.localScale.x, _crouchYScale, transform.localScale.z);
        _isCrouching = true;
        
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        _moveSpeed = crouchSpeed;
    }

    private void StopCrouch() {
        transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
        _isCrouching = false;
        _moveSpeed = walkSpeed;
    }
    
    private void InputHandler() {
        _horizontalMov = Input.GetAxisRaw("Horizontal");
        _verticalMov = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && _isReadyToJump && _isGrounded) {
            Jump();
            _isReadyToJump = false;
            StartCoroutine(JumpReset(jumpCooldown));
        }
        
        if (Input.GetKeyDown(crouchKey)) {
            BeginCrouch();
        } 
        
        if (Input.GetKeyUp(crouchKey)) {
            StopCrouch();
        }
    }

    private void VelocityHandler() {
        Vector3 velocity = _rb.velocity;
        Vector3 flatVelocity = new Vector3(velocity.x, 0.0f, velocity.z);

        if (flatVelocity.magnitude > _moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _moveSpeed;
            _rb.velocity = new Vector3(limitedVelocity.x, velocity.y, limitedVelocity.z);
        }
    }
}
