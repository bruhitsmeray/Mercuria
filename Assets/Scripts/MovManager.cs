using System;
using UnityEngine;
using UnityEngine.UI;

public class MovManager : MonoBehaviour
{
    [Header("Speeds")]
    private float _moveSpeed;
    public float walkSpeed;
    public float runSpeed;

    [Header("Physics")]
    public float groundDrag;

    private float _horizontalMov;
    private float _verticalMov;

    public Transform orientation;
    private Vector3 _direction;
    private Rigidbody _rb;

    [Header("GroundCheck")] 
    public float height;
    public LayerMask groundMask;
    private bool _isGrounded;
    
    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _moveSpeed = walkSpeed;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, groundMask);
        
        InputHandler();

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
        _direction = orientation.forward * _verticalMov + orientation.right * _horizontalMov;
        _rb.AddForce(_direction.normalized * (_moveSpeed * 10.0f), ForceMode.Force);
    }

    private void InputHandler() {
        _horizontalMov = Input.GetAxisRaw("Horizontal");
        _verticalMov = Input.GetAxisRaw("Vertical");
    }

    private void Run() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            _moveSpeed = walkSpeed * 1.5f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            _moveSpeed = walkSpeed;
        }
    }
}
