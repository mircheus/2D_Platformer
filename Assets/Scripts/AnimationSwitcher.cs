using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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
    private float _horizontalMovement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        MovePlayer();
        Jump();
    }

    private void MovePlayer()
    {
        _animator.SetBool(_isJumping, !IsGrounded());
        
        if (_horizontalMovement > 0)
        {
            _animator.SetBool(_isMoving, true);
            _spriteRenderer.flipX = false;
        }
        else if (_horizontalMovement < 0)
        {
            _animator.SetBool(_isMoving, true);
            _spriteRenderer.flipX = true;
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundChecker.position, 0.5f, _groundLayer);
    }
}