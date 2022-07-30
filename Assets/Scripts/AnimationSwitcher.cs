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
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private int _isMoving = Animator.StringToHash("isMoving");
    private int _isJumping = Animator.StringToHash("isJumping");
    private float _horizontalMovement;
    private bool _isGrounded;
    
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
           _isGrounded = true;
           _animator.SetBool(_isJumping, false);
        }
    }

    private void MovePlayer()
    {
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

        if (_isGrounded)
        {
            _animator.SetBool(_isJumping, false);
        }
        else
        {
            _animator.SetBool(_isJumping, true);
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _animator.SetBool(_isJumping, true);
            _isGrounded = false;
        }
    }
}