using System;
using System.Collections.Generic;
using UnityEngine;

public struct StateChangeEvent<T> where T : struct, IComparable, IConvertible, IFormattable
{
    public GameObject Target;
    public StateMachine<T> TargetStateMachine;
    public T NewState;
    public T PreviousState;

    public StateChangeEvent(StateMachine<T> stateMachine)
    {
        Target = stateMachine.Target;
        TargetStateMachine = stateMachine;
        NewState = stateMachine.CurrentState;
        PreviousState = stateMachine.PreviousState;
    }
}

public class StateMachine<T> where T : struct, IComparable, IConvertible, IFormattable
{
    public GameObject Target;
    public T CurrentState { get; protected set; }
    public T PreviousState { get; protected set; }

    public delegate void OnStateChangeDelegate();

    public OnStateChangeDelegate OnStateChange;

    public StateMachine(GameObject target)
    {
        this.Target = target;
    }

    public virtual void ChangeState(T newState)
    {
        // if the new state is the current one, do nothing and exit
        if (EqualityComparer<T>.Default.Equals(newState, CurrentState))
        {
            return;
        }

        PreviousState = CurrentState;
        CurrentState = newState;
        OnStateChange?.Invoke();
    }

    public virtual void RestorePreviousState()
    {
        CurrentState = PreviousState;
        OnStateChange?.Invoke();
    }
}
