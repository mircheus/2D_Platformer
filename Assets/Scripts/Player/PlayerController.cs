using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{ 
    [Header("Layer Masks")] 
    [SerializeField] private LayerMask _groundLayer;
    
    [Header("Components")] 
    private Rigidbody2D _rigidbody;

    [Header("Movement Variables")] 
    [SerializeField] private float _movementAcceleration = 50f;
    [SerializeField] private float _maxMoveSpeed = 10f;
    [SerializeField] private float _linearDrag = 7f; // Deacceleration или замедление персонажа

    [Header("Ground Checker")] 
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _circleRadius = 0.5f;

    [Header("Jump Variables")] 
    [SerializeField] private float _jumpForce = 12f;   
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 14f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;

    [Header("Wall Slide")] 
    // [SerializeField] private Transform _wallChecker;
    [SerializeField] private BoxCollider2D _wallCheckerCollider;
    [SerializeField] private Vector2 _offsetVector;
    [SerializeField] private float _wallSlidingSpeed;
        
    [Header("Wall Jump")] 
    [SerializeField] private float _xWallForce;
    [SerializeField] private float _yWallForce;
    [SerializeField] private float _wallJumpTime;
    
    [SerializeField] private ContactFilter2D _groundContactFilter;

    [Header("Jump Shoot Statter")] 
    [SerializeField] private float _jumpStatterForce;
    [SerializeField] private PlayerShooting _player;
    
    [Header("Debug variables")]
    [SerializeField] private bool _isTouchingWall;
    [SerializeField] private bool _wallSliding;
    [SerializeField] private bool _wallJumping;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isAbleToMove = true;
    [SerializeField] private float _horizontalDirectionRaw;
    [SerializeField] private float _verticalDirectionRaw;
    // private Vector2 _wallCheckerOffsetRight = new Vector2(0.3f, -0.1f);
    // private Vector2 _wallCheckerOffsetLeft = new Vector2(-0.3f, -0.1f);


    private bool ChangingDirection => (_rigidbody.velocity.x > 0f && _horizontalDirectionRaw < 0f) ||
                                      (_rigidbody.velocity.x < 0f && _horizontalDirectionRaw > 0f); // Избавляемся от скольжения при повороте персонажа 
    
    private bool _isAbleToJump => Input.GetKeyDown(KeyCode.Space) && IsGrounded();
    
    private void OnEnable()
    {
        _player.Shooted += JumpShootStatter;
    }

    private void OnDisable()
    {
        _player.Shooted -= JumpShootStatter;
    }
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GetComponent<PlayerShooting>();
        // _wallCheckerCollider = _wallChecker.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _horizontalDirectionRaw = GetInputRaw().x;
        _verticalDirectionRaw = GetInputRaw().y;
        _isGrounded = IsGrounded();
        _isTouchingWall = IsTouchingWall();
        
        SetWallCheckerDirection(_horizontalDirectionRaw);
        
        if (_isAbleToJump)
        {
            Jump();
        }


        if (IsTouchingWall() && !IsGrounded() && _horizontalDirectionRaw != 0)
        {
            _wallSliding = true;
        }
        else
        {
            _wallSliding = false;
        }

        if (_wallSliding)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x,Mathf.Clamp(_rigidbody.velocity.y, -_wallSlidingSpeed, float.MaxValue ));
        }
        
        if (_verticalDirectionRaw > 0.1f && _wallSliding && Input.GetKeyDown(KeyCode.Space))
        {
            _wallJumping = true;
            Invoke(nameof(SetWallJumpingToFalse), _wallJumpTime);
            
            StopCoroutine(DisableMovement(0f));
            StartCoroutine(DisableMovement(.4f));
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(new Vector2(_xWallForce * -_horizontalDirectionRaw, _yWallForce), ForceMode2D.Impulse);
        }
        
        // if (_wallJumping)
        // {
        //     // _rigidbody2D.velocity = new Vector2(_xWallForce * -_horizontalDirection, _yWallForce);
        //     _rigidbody2D.AddForce(new Vector2(_xWallForce * -_horizontalDirectionRaw, _yWallForce), ForceMode2D.Impulse);   
        //     // Debug.Log($"{_rigidbody2D.velocity.x}");
        // }
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

        if (_wallSliding)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x,
                Mathf.Clamp(_rigidbody.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
    }
    
    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void ApplyFallMultiplier()
    {
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.gravityScale = _fallMultiplier;
        }
        else if (_rigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            _rigidbody.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rigidbody.gravityScale = 1f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _rigidbody.drag = _airLinearDrag;
    }

    private Vector2 GetInputRaw()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void Move()
    {
        if (!_isAbleToMove)
        {
            return;
        }
        
        _rigidbody.AddForce(new Vector2(_horizontalDirectionRaw, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rigidbody.velocity.x) > _maxMoveSpeed) // Ограничиваем максимальную скорость персонажа
        {
            _rigidbody.velocity =
                new Vector2(Mathf.Sign(_rigidbody.velocity.x) * _maxMoveSpeed, _rigidbody.velocity.y);
        }
    }
    
    private void ApplyGroundLinearDrag() // Делаем так чтобы персонаж не скользил постоянно по плоскости
    {
        float minHorizontalDirectionValue = 0.4f;
        
        if (Mathf.Abs(_horizontalDirectionRaw) < minHorizontalDirectionValue || ChangingDirection)
        {
            _rigidbody.drag = _linearDrag;
        }
        else
        {
            _rigidbody.drag = 0f; 
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundChecker.position, _circleRadius, _groundLayer);;
        // Здесь проверяется попадает ли в радиус круга объект из слоя Ground
    }

    private bool IsTouchingWall()
    {
        return _wallCheckerCollider.IsTouching(_groundContactFilter);
    }

    private void SetWallCheckerDirection(float horizontalDirection)
    {
        if (horizontalDirection > 0.1f)
        {
            _wallCheckerCollider.offset = _offsetVector;
        }
        else if (horizontalDirection < -0.1f)
        {
            _wallCheckerCollider.offset = -_offsetVector;
        }
        else if (horizontalDirection == 0)
        {
            _wallCheckerCollider.offset = Vector2.zero;
        }
    }

    private bool IsOnWall()
    {
        return false;
    }

    private IEnumerator DisableMovement(float time)
    {
        _isAbleToMove = false;
        yield return new WaitForSeconds(time);
        _isAbleToMove = true;
    }

    private void SetWallJumpingToFalse()
    {
        _wallJumping = false;
    }
    
    private void JumpShootStatter()
    {
        if (_isGrounded == false)
        {
            // _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpStatterForce);
            _rigidbody.AddForce(Vector2.up * _jumpStatterForce, ForceMode2D.Impulse);
            Debug.Log("Statter triggered");
        }
    }
    
    private void OnDrawGizmos()
    {
        // Handles.color = IsTouchingWall() ? Color.green : Color.red;
        // Handles.DrawWireCube(_wallChecker.position, new Vector2(_sizeByX, _sizeByY));

        // Handles.color = IsGrounded() ? Color.green : Color.red;
        // Handles.DrawWireDisc(_groundChecker.position, transform.forward, _circleRadius, 2f);
    }
}
