using System;
using UnityEngine;

public abstract class BaseState : IState
{
    protected StateMachine _stateMachine;
    
    public BaseState(StateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }
    
    public virtual void OnEnter()
    {
    }
    
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    
    public virtual void OnExit()
    {
    }

    protected void ChangeState<T>() where T : IState
    {
        _stateMachine.ChangeState<T>();
    }

    protected void ChangeState<T>(Action<T> onStateInit) where T : IState
    {
        _stateMachine.ChangeState<T>(onStateInit);
    }
}
