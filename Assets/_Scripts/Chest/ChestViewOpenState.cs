using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestViewOpenState : BaseState
{
    ChestViewManager _chestViewManager;
    ChestViewFSM _chestViewFSM;

    public ChestViewOpenState(ChestViewFSM chestViewFSM, ChestViewManager chestViewManager)
    {
        _chestViewFSM = chestViewFSM;
        _chestViewManager = chestViewManager;
    }

    public override void Enter()
    {
        _chestViewManager.SetChestView();
        _chestViewManager.AnimateChestViewOpen();
    }

    public override void Exit()
    {
        _chestViewManager.AnimateChestViewClose();
    }
}
