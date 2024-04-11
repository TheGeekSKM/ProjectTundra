using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonCombatState : BaseState
{
    CombatManager _combatManager;
    CombatFSM _combatFSM;

    public NonCombatState(CombatManager combatManager, CombatFSM combatFSM)
    {
        _combatManager = combatManager;
        _combatFSM = combatFSM;
    }

    public override void Enter()
    {
        _combatManager.NonCombat();
        _combatManager.AnimatePlayerControlsOutro();
        _combatManager.AnimatePlayerMovementControlsIntro();
        // Debug.Log("NonCombatState Enter");
    }

    public override void Exit()
    {
        // Debug.Log("NonCombatState Exit");
    }
}
