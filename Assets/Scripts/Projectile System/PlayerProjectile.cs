using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
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
