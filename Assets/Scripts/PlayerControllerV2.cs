using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerV2 : MonoBehaviour
{
    [Header("Layer Masks")] 
    [SerializeField] private LayerMask _groundLayer;
    
    [Header("Components")] 
    private Rigidbody2D _rigidbody2D;

    [Header("Movement Variables")] 
    [SerializeField] private float _movementAcceleration = 50f;
    [SerializeField] private float _maxMoveSpeed = 10f;
    [SerializeField] private float _linearDrag = 7f;
    
    private float _horizontalDirection;
    
    [Header("Ground Checker")] 
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _circleRadius = 0.5f;

    [Header("Jump Variables")] 
    [SerializeField] private float _jumpForce = 12f;   
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 14f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;

    private bool ChangingDirection => (_rigidbody2D.velocity.x > 0f && _horizontalDirection < 0f) ||
                                       (_rigidbody2D.velocity.x < 0f && _horizontalDirection > 0f);
    
    private bool CanJump => Input.GetKeyDown(KeyCode.Space) && IsGrounded();

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;
        if (CanJump) Jump();
    }

    private void FixedUpdate()
    {
        Move();
        
        if (IsGrounded())
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            ApplyFallMultiplier();
        }
    }
    
    private void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void ApplyFallMultiplier()
    {
        if (_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.gravityScale = _fallMultiplier;
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            _rigidbody2D.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rigidbody2D.gravityScale = 1f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _rigidbody2D.drag = _airLinearDrag;
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void Move()
    {
        _rigidbody2D.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rigidbody2D.velocity.x) > _maxMoveSpeed)
        {
            _rigidbody2D.velocity =
                new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) * _maxMoveSpeed, _rigidbody2D.velocity.y);
        }
    }
    
    private void ApplyGroundLinearDrag()
    {
        float minHorizontalDirectionValue = 0.4f;
        
        if (Mathf.Abs(_horizontalDirection) < minHorizontalDirectionValue || ChangingDirection)
        {
            _rigidbody2D.drag = _linearDrag;
        }
        else
        {
            _rigidbody2D.drag = 0f; 
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundChecker.position, _circleRadius, _groundLayer);
    }
}
