using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationSwitcher : MonoBehaviour
{
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _groundLayer;
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private int _isMoving = Animator.StringToHash("isMoving");
    private int _isJumping = Animator.StringToHash("isJumping");
    private int _isShooting = Animator.StringToHash("isShooting");
    private int _isAttacking = Animator.StringToHash("isAttacking");
    private int _isFiring = Animator.StringToHash("isFiring");
    private float _horizontalMovement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        Move();
        Jump();
        Shoot();
        Attack();
    }

    private void Move()
    {
        _animator.SetBool(_isJumping, !IsGrounded());
        
        if (_horizontalMovement > 0)
        {
            _animator.SetBool(_isMoving, true);
            TurnCharacterRight();
        }
        else if (_horizontalMovement < 0)
        {
            _animator.SetBool(_isMoving, true);
            TurnCharacterLeft();
        }
        else if (_horizontalMovement == 0)
        {
            _animator.SetBool(_isMoving, false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool(_isJumping, true);
        }
    }

    // используется в Kinx и Knightside
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // _animator.SetBool(_isShooting, true);
            _animator.SetTrigger(_isFiring);
            TurnCharacterLeft();
        }
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     _animator.SetBool(_isShooting, false);
        //     TurnCharacterLeft();
        // }

        if (Input.GetMouseButtonDown(1))
        {
            // _animator.SetBool(_isShooting, true);
            _animator.SetTrigger(_isFiring);
            TurnCharacterRight();
        }

        // if (Input.GetMouseButtonUp(1))
        // {
        //     _animator.SetBool(_isShooting, false);
        //     TurnCharacterRight();
        // }
    }

    
    // используется только в Knightside
    private void Attack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool(_isAttacking, true);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            _animator.SetBool(_isAttacking, false);
        }
    }

    private void TurnCharacterLeft()
    {
        _spriteRenderer.flipX = true;
    }

    private void TurnCharacterRight()
    {
        _spriteRenderer.flipX = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundChecker.position, 0.5f, _groundLayer);
    }
}