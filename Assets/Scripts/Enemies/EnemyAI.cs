using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro.EditorUtilities;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed;
    public float nextWayPointDistance;

    private Path _path;
    private int _currentWayPoint = 0;
    private bool _isReachedEndOfPath = false;

    private Seeker _seeker;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rigidbody = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if (_seeker.IsDone())
        {
            _seeker.StartPath(_rigidbody.position, target.position, OnPathComplete);
        }
    }

    private void FixedUpdate()
    {
        if (_path == null)
        {
            return;
        }

        if (_currentWayPoint >= _path.vectorPath.Count)
        {
            _isReachedEndOfPath = true;
            return;
        }
        else
        {
            _isReachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWayPoint] - _rigidbody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        
        _rigidbody.AddForce(force);

        float distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[_currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            _currentWayPoint++;
        }
    }

    private void OnPathComplete(Path path)
    {
        if (path.error == false)
        {
            _path = path;
            _currentWayPoint = 0;
        }   
    }
}
