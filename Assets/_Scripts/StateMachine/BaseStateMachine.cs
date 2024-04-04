using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    public BaseState CurrentState { get; private set; }
    public BaseState PreviousState { get; private set; }

    bool _inTransition = false;

    void Update()
    {
        Tick();
    }

    void FixedUpdate()
    {
        FixedTick();
    }

    public void ChangeState(BaseState newState)
    {
        if (CurrentState == newState || _inTransition)
            return;

        ChangeStateSequence(newState);
    }

    public void ChangeState(BaseState newState, float delayBeforeChange, float delayAfterChange)
    {
        StartCoroutine(ChangeStateDelayed(newState, delayBeforeChange, delayAfterChange));
    }

    IEnumerator ChangeStateDelayed(BaseState newState, float delayBeforeChange, float delayAfterChange)
    {
        yield return new WaitForSeconds(delayBeforeChange);
        ChangeStateSequence(newState);
        yield return new WaitForSeconds(delayAfterChange);
    }

    void ChangeStateSequence(BaseState baseState)
    {
        _inTransition = true;

        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        PreviousState = CurrentState;
        CurrentState = baseState;

        if (CurrentState != null)
        {
            CurrentState.Enter();
        }

        _inTransition = false;
    }

    public void ReverseToPreviousState()
    {
        if (PreviousState != null)
        {
            ChangeState(PreviousState);
        }
    }

    public void Tick()
    {
        if (CurrentState != null && !_inTransition)
        {
            CurrentState.Tick();
        }
    }

    public void FixedTick()
    {
        if (CurrentState != null && !_inTransition)
        {
            CurrentState.FixedTick();
        }
    }
}
