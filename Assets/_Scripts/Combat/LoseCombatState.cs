using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCombatState : BaseState
{
    CombatManager _combatManager;
    CombatFSM _combatFSM;

    public LoseCombatState(CombatManager combatManager, CombatFSM combatFSM)
    {
        _combatManager = combatManager;
        _combatFSM = combatFSM;
    }

    public override void Enter()
    {
        _combatManager.LoseCombat();
        Debug.Log("LoseCombatState Enter");
    }

}
