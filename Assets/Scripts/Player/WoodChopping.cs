using System;
using System.Collections;
using System.Collections.Generic;



using UnityEngine;

public class WoodChopping : MonoBehaviour
{
    [SerializeField] private Transform _rightChopPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _woodLayer;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Chop();
        }
    }

    private void Chop()
    {
        Collider2D[] hitWood = Physics2D.OverlapCircleAll(_rightChopPoint.position, _attackRange, _woodLayer);

        foreach (Collider2D log in hitWood)
        {
            log.GetComponent<Wood>().GetChopped();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_rightChopPoint.position, _attackRange);
    }
}
