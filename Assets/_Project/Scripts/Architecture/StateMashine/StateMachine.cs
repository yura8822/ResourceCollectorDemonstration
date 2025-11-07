using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

    public event System.Action<IState, IState> OnStateChanged;

    void Update()
    {
        _currentState?.OnUpdate();
    }

    void FixedUpdate()
    {
        _currentState?.OnFixedUpdate();
    }

    /// <summary>
    /// Registers the new state of the machine in the machine
    /// </summary>
    public void RegisterState<T>(T state) where T : IState
    {
        Type stateType = typeof(T);

        if (!_states.ContainsKey(stateType))
        {
            _states.Add(stateType, state);
        }
        else
        {
            Debug.LogWarning($"State {stateType.Name} already registered!");
        }
    }

    /// <summary>
    /// Changes the state to the specified type
    /// </summary>
    public void ChangeState<T>() where T : IState
    {
        Type stateType = typeof(T);

        if (_states.ContainsKey(stateType))
        {
            IState newState = _states[stateType];

            if (_currentState == newState)
            {
                return;
            }
            
            // Уведомление о смене состояния
            OnStateChanged?.Invoke(_currentState, newState);

            // Выход из текущего состояния
            _currentState?.OnExit();
            // Смена состояния
            _currentState = newState;
            // Вход в новое состояние
            _currentState.OnEnter();
        }
        else
        {
            Debug.LogError($"State {stateType.Name} not registered!");
        }
    }

    /// <summary>
    /// Changes the state to the specified type with optional initialization callback
    /// </summary>
    /// <typeparam name="T">The state type to change to</typeparam>
    /// <param name="onStateInit">Optional callback to initialize state before OnEnter is called</param>
    public void ChangeState<T>(Action<T> onStateInit) where T : IState
    {
        Type stateType = typeof(T);

        if (_states.ContainsKey(stateType))
        {
            IState newState = _states[stateType];

            if (_currentState == newState)
            {
                return;
            }
            
            // Уведомление о смене состояния
            OnStateChanged?.Invoke(_currentState, newState);

            // Выход из текущего состояния
            _currentState?.OnExit();
            // Смена состояния
            _currentState = newState;
            
            // Вызываем инициализацию перед OnEnter
            onStateInit?.Invoke((T)_currentState);
            
            // Вход в новое состояние
            _currentState.OnEnter();
        }
        else
        {
            Debug.LogError($"State {stateType.Name} not registered!");
        }
    }


    /// <summary>
    /// Gets the current state
    /// </summary>
    public T GetCurrentState<T>() where T : class, IState
    {
        return _currentState as T;
    }

    /// <summary>
    /// Checks if the machine is in the specified state
    /// </summary>
    public bool IsInState<T>() where T : IState
    {
        return _currentState is T;
    }

    /// <summary>
    /// Gets all registered states
    /// </summary>
    public IEnumerable<Type> GetRegisteredStates()
    {
        return _states.Keys;
    }

    /// <summary>
    /// Starts the state machine with the initial stateм
    /// </summary>
    public void StartStateMachine<T>() where T : IState
    {
        if (_states.Count == 0)
        {
            Debug.LogError("No states registered! Register states before starting.");
            return;
        }

        ChangeState<T>();
    }
}