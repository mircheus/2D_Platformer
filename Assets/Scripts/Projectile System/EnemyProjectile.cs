using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void Update()
    {
        Move();
    }
    
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
