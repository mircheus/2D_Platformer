using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FlashEffect))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _explosionFx;

    private FlashEffect _damageFx;
    public event UnityAction Damaged;
    
    private void Start()
    {
        _damageFx = GetComponent<FlashEffect>();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _damageFx.Blink();
        Damaged?.Invoke();

        Debug.Log($"-{damage}!");
        
        if (_health <= 0)
        {
            Destroy(gameObject);
            Instantiate(_explosionFx, transform.position, quaternion.identity);
        }
    }
}
