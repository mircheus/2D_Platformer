using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip _pickupSound;
    [SerializeField] private GameObject _pickupVFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerControllerV2>(out var player))
        {
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
            Instantiate(_pickupVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
 