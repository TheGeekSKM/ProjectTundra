using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCombatState : BaseState
{
    CombatManager _combatManager;
    CombatFSM _combatFSM;

    public WinCombatState(CombatManager combatManager, CombatFSM combatFSM)
    {
        _combatManager = combatManager;
        _combatFSM = combatFSM;
    }

    public override void Enter()
    {
        _combatManager.WinCombat();
        Debug.Log("WinCombatState Enter");
    }

    public override void Exit()
    {
        Debug.Log("WinCombatState Exit");
    }
}
