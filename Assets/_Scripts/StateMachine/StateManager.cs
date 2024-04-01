using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, State<EState>> States = new Dictionary<EState, State<EState>>();
    protected State<EState> CurrentState;

    protected bool IsTransitioningState = false;

    void Start()
    {
        CurrentState.EnterState();    
    }

    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CurrentState.OnTriggerExit(other);
    }

    public void TransitionToState(EState nextStateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[nextStateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }
}
