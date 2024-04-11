using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestViewFSM : BaseStateMachine
{
    ChestViewManager _chestViewManager;
    public ChestViewOpenState OpenState { get; private set; }
    public ChestViewClosedState ClosedState { get; private set; }

    void Awake()
    {
        if (_chestViewManager == null)
        {
            _chestViewManager = GetComponent<ChestViewManager>();
        }
        OpenState = new ChestViewOpenState(this, _chestViewManager);
        ClosedState = new ChestViewClosedState(this, _chestViewManager);
    }

    void Start()
    {
        ChangeState(ClosedState);
    }

}
