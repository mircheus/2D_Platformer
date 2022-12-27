using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectileTEST : Projectile
{
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _speed = 1f;
        _damage = 69;
        SetDirection(Vector2.left);
        // в таком сетапе всё работает, проджектайл движется
    }

    private void Update()
    {
        Move();
    }
    //
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
            ReturnToPool();
        }
        
        if (col.gameObject.layer == 3) // надо это как-то вынести в общий метод projectile
        {
            ReturnToPool();
        }
    }
}
