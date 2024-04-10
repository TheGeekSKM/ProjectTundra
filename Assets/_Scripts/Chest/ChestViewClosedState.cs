using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestViewClosedState : BaseState
{
    ChestViewManager _chestViewManager;
    ChestViewFSM _chestViewFSM;

    public ChestViewClosedState(ChestViewFSM chestViewFSM, ChestViewManager chestViewManager)
    {
        _chestViewFSM = chestViewFSM;
        _chestViewManager = chestViewManager;
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}
