using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : BaseState
{
    CombatManager _combatManager;
    CombatFSM _combatFSM;

    public PlayerCombatState(CombatManager combatManager, CombatFSM combatFSM)
    {
        _combatManager = combatManager;
        _combatFSM = combatFSM;
    }

    public override void Enter()
    {
        _combatManager.PlayerTurnIntro();
        _combatManager.AnimatePlayerControlsIntro();
    }

    public override void Exit()
    {
        _combatManager.AnimatePlayerControlsOutro();
        _combatManager.AnimatePlayerMovementControlsOutro();
        // Debug.Log("PlayerCombatState Exit");
    }
}
