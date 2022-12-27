using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    protected float _speed;
    protected int _damage;
    protected Rigidbody2D _rigidbody;
    private Vector2 _currentDirection;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        Move();
    }

    public void Initialize(float speed, int damage)
    {
        _speed = speed;
        _damage = damage;
    }

    public void SetDirection(Vector2 direction)
    {
        _currentDirection = direction;
    }

    protected void Move()
    {
        // transform.Translate(Vector3.right * _speed * Time.deltaTime);
        _rigidbody.velocity = _currentDirection * _speed;
    }
    
    protected void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}