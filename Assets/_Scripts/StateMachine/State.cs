using System;
using UnityEngine;

public abstract class State<EState> where EState : Enum
{

    public State(EState key)
    {
        StateKey = key;
    }

    public EState StateKey { get; private set; }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    public abstract void OnTriggerEnter(Collider2D other);
    public abstract void OnTriggerStay(Collider2D other);
    public abstract void OnTriggerExit(Collider2D other);    
}
