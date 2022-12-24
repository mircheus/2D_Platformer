using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MeleeBaseState : State
{
    // how long this state should be active for before moving on
    public float Duration;
    protected Animator _animator;
    protected bool _shouldCombo;
    protected int _attackIndex;

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        _animator = GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetMouseButtonDown(0))
        {
            _shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
