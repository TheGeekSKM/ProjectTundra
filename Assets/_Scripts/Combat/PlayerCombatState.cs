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
        Debug.Log("PlayerCombatState Enter");
    }

    public override void Exit()
    {
        Debug.Log("PlayerCombatState Exit");
    }
}
