using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatFSM : BaseStateMachine
{
    [SerializeField] private CombatManager _combatManager;
    public CombatManager CombatManager => _combatManager;


    public NonCombatState NonCombatState { get; private set; }
    public PlayerCombatState PlayerCombatState { get; private set; }
    public EnemyCombatState EnemyCombatState { get; private set; }
    public LoseCombatState LoseCombatState { get; private set; }
    public WinCombatState WinCombatState { get; private set; }


    void OnValidate()
    {
        if (_combatManager == null)
        {
            _combatManager = GetComponent<CombatManager>();
        }
    }

    void Awake()
    {
        NonCombatState = new NonCombatState(_combatManager, this);
        PlayerCombatState = new PlayerCombatState(_combatManager, this);
        EnemyCombatState = new EnemyCombatState(_combatManager, this);
        LoseCombatState = new LoseCombatState(_combatManager, this);
        WinCombatState = new WinCombatState(_combatManager, this);
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        ChangeState(NonCombatState);
    }
}
