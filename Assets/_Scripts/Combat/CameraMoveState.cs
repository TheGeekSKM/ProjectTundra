using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveState : BaseState
{
    CombatManager _combatManager;
    CombatFSM _combatFSM;

    public CameraMoveState(CombatManager combatManager, CombatFSM combatFSM)
    {
        _combatManager = combatManager;
        _combatFSM = combatFSM;
    }

    public override void Enter()
    {
        _combatManager.CameraMove();
        _combatManager.AnimatePlayerControlsOutro();
        _combatManager.AnimatePlayerMovementControlsOutro();
    }

    public override void Exit()
    {

    }
}
