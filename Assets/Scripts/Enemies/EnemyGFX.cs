using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// script for drone sprite flip while chasing the player
public class EnemyGFX : MonoBehaviour
{
    public AIPath AIPath;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // мой вариант через флип
        if (AIPath.desiredVelocity.x >= 0.01f)
        {
            _spriteRenderer.flipX = true;
        }
        else if (AIPath.desiredVelocity.x <= -0.01f)
        {
            _spriteRenderer.flipX = false;
        }
        
        // вариант Brackeys через localscale
        // if (AIPath.desiredVelocity.x >= 0.01f)
        // {
        //     transform.localScale = new Vector3(-1f, 1f, 1f);
        // }
        // else if (AIPath.desiredVelocity.x <= -0.01f)
        // {
        //     transform.localScale = new Vector3(1f, 1f, 1f);
        // }
        
    }
}
