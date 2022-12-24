using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;

public class PlayerShooting : ProjectilePool
{
    [SerializeField] private Transform _rightShootPoint;
    [SerializeField] private Transform _leftShootPoint;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private int _damage;

    public event UnityAction Shooted;
    
    private void Start()
    {
        Initialize(_projectilePrefab, _projectileSpeed, _damage);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootInDirection(Vector2.left, _leftShootPoint);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ShootInDirection(Vector2.right, _rightShootPoint);
        }
    }

    private void ShootInDirection(Vector2 direction, Transform shootPoint)
    {
        if (TryGetProjectile(out Projectile projectile))
        {
            projectile.transform.position = shootPoint.position;
            projectile.SetDirection(direction);
            EnableObject(projectile);
            Shooted?.Invoke();
        }
    }
}
