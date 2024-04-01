using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState
{
    public float StateDuration { get; private set; } = 0f;
    public virtual void Enter()
    {
        StateDuration = 0f;
    
    }
    public virtual void Tick()
    {
        StateDuration += Time.deltaTime;
    }
    public virtual void FixedTick()
    {
        StateDuration += Time.fixedDeltaTime;
    }
    public virtual void Exit()
    {
        StateDuration = 0f;
    }

}
