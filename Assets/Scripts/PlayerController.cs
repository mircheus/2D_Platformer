using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
   [SerializeField] private float _movementSpeed = 3.0f;
   [SerializeField] private float _jumpForce = 3.0f;

   private Animator _animator;
   private AudioSource _audioSource;
   private Rigidbody2D _rigidbody2D;
   private SpriteRenderer _spriteRenderer;
   private float _horizontalMovement;
   private bool _isGrounded = true;
   private bool _justJumped = false;

   private readonly int _isJumping = Animator.StringToHash("isJumping");
   private readonly int _isMoving = Animator.StringToHash("isMoving");
   
   private void Start()
   {
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _spriteRenderer = GetComponent<SpriteRenderer>();
      _animator = GetComponent<Animator>();
      _audioSource = GetComponent<AudioSource>();
   }

   private void Update()
   {
      _horizontalMovement = Input.GetAxisRaw("Horizontal");
      MovePlayerViaTranslate();
   }
   
   private void FixedUpdate()
   {
      if (_justJumped)
      {
         _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
         _justJumped = false;
      }
   }
   
   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.gameObject.CompareTag("Ground"))
      {
         _isGrounded = true;
         _animator.SetBool(_isJumping, false);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if(other.gameObject.TryGetComponent<Coin>(out Coin coin))
      {
         Debug.Log("Coin picked up");
         _audioSource.Play();
      }
   }

   private void MovePlayerViaTranslate()
   {
      transform.Translate(Vector2.right * _horizontalMovement * _movementSpeed * Time.deltaTime);
        
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
      
      if (_justJumped == false && Input.GetKeyDown(KeyCode.Space) && _isGrounded)
      {
         _animator.SetBool(_isJumping, true);
         _justJumped = true;
         _isGrounded = false;
      }
   }
}
