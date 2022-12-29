using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerDetection : MonoBehaviour
{
    public event UnityAction PlayerDetected;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Player player))
        {
            PlayerDetected?.Invoke();
            // Debug.Log("Player detected");
        }
    }
}

