using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{ 
    private void OnEnable()
    {
        // _rigidbody = GetComponent<Rigidbody2D>();
        // _rigidbody.velocity = (Vector2.up) * _speed;
        Debug.Log("Player projectile enabled");
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
            ReturnToPool();
        }
        
        if (col.gameObject.layer == 3) // надо это как-то вынести в общий метод projectile
        {
            ReturnToPool();
        }
    }
}
