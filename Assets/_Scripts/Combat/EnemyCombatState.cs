using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : BaseState
{
    CombatManager _combatManager;
    CombatFSM _combatFSM;

    public EnemyCombatState(CombatManager combatManager, CombatFSM combatFSM)
    {
        _combatManager = combatManager;
        _combatFSM = combatFSM;
    }

    public override void Enter()
    {
        base.Enter();
        _combatManager.EnemyTurnIntro();
        Debug.Log("EnemyCombatState Enter");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("EnemyCombatState Exit");
    }
}

