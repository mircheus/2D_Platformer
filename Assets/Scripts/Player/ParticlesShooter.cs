using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesShooter : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystemRenderer _particleSystemRenderer;
    private float _inputX;

    private void Start()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetMouseButtonDown(1))
        {
            _particleSystem.Play();
        }

        if (Input.GetMouseButtonUp(1))
        {
            _particleSystem.Stop();
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _particleSystemRenderer.gameObject.SetActive(true);
        }
    
        if (_inputX < 0)
        {
            _particleSystemRenderer.flip = Vector3.right;
            Debug.LogWarning($"PSR flip = {_particleSystemRenderer.flip}");
        }
        else if (_inputX > 0)
        {
            _particleSystemRenderer.flip = Vector3.left;
            Debug.LogWarning($"PSR flip = {_particleSystemRenderer.flip}"); 
        }
    }
}
