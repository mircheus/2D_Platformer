using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void OnEnable()
    {
        // _rigidbody = GetComponent<Rigidbody2D>();
        // _rigidbody.velocity = (Vector2.up) * _speed;
        Debug.Log("turret projectile enabled");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
            ReturnToPool();
        }
        
        // if (col.gameObject.layer == 3) // надо это как-то вынести в общий метод projectile
        // {
        //     ReturnToPool();
        // }
    }
}
