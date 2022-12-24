using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooting : ProjectilePool
{
    // [SerializeField] private Transform _rightShootPoint;
    // [SerializeField] private Transform _leftShootPoint;
    [SerializeField] private Transform _currentShootPoint;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private PlayerDetection _detector;
    [SerializeField] private float _shootPauseDuration;

    [Range(-1, 1)] 
    [SerializeField] private int _shootDirection;

    // private void OnEnable()
    // {
    //     _detector.PlayerDetected += OnPlayerDetected;
    // }
    //
    // private void OnDisable()
    // {
    //     _detector.PlayerDetected -= OnPlayerDetected;
    // }

    private void Start()
    {
        Initialize(_projectilePrefab, _projectileSpeed, _damage);
        StartCoroutine(Firing());
        // ShootProjectile();
    }
    
    
    private void OnPlayerDetected()
    {
        // StopCoroutine(Firing());
        // StartCoroutine(Firing());
        // ShootProjectile();
    }

    private void ShootProjectile()
    {
        if (TryGetProjectile(out Projectile projectile))
        {
            projectile.transform.position = _currentShootPoint.position;
            projectile.SetDirection(Vector2.right * _shootDirection); 
            EnableObject(projectile);
            Debug.Log("Projectile activated");
        }
    }

    private IEnumerator Firing()
    {
        var waitFor = new WaitForSeconds(_shootPauseDuration);
        int i = 0;
        
        while (i < 100)
        {
            ShootProjectile();
            Debug.Log("Fired");
            yield return waitFor;
        }
    }
    
}
