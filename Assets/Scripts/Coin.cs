using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip _pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerControllerV2>())
        {
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}
 