using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatTurnState
{
    NonCombat,
    Player,
    Enemy,
    Room,
    Win,
    Lose

}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    [SerializeField] CombatFSM combatFSM;

    [SerializeField] private List<EntityStatsContainer> _enemies;
    public List<EntityStatsContainer> Enemies => _enemies;

    [SerializeField] CombatTurnState _currentTurnState;
    public event System.Action<CombatTurnState> OnTurnChanged;

    void OnValidate()
    {
        if (combatFSM == null) combatFSM = GetComponent<CombatFSM>();
        if (combatFSM == null) combatFSM = gameObject.AddComponent<CombatFSM>();
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddEnemy(EntityStatsContainer enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(EntityStatsContainer enemy)
    {
        _enemies.Remove(enemy);
        WinCheck();
    }

    void WinCheck()
    {
        if (_enemies.Count <= 0)
        {
            _currentTurnState = CombatTurnState.Win;
            OnTurnChanged?.Invoke(CombatTurnState.Win);
        }
    }

    public void PlayerTurnIntro()
    {
        _currentTurnState = CombatTurnState.Player;
        OnTurnChanged?.Invoke(CombatTurnState.Player);
    }

    public void EnemyTurnIntro()
    {
        _currentTurnState = CombatTurnState.Enemy;
        OnTurnChanged?.Invoke(CombatTurnState.Enemy);
    }

    public void RoomTurnIntro()
    {
        _currentTurnState = CombatTurnState.Room;
        OnTurnChanged?.Invoke(CombatTurnState.Room);
    }

    public void LoseCombat()
    {
        _currentTurnState = CombatTurnState.Lose;
        OnTurnChanged?.Invoke(CombatTurnState.Lose);
    }

    public void WinCombat()
    {
        _currentTurnState = CombatTurnState.Win;
        OnTurnChanged?.Invoke(CombatTurnState.Win);
    }

    public void NonCombat()
    {
        _currentTurnState = CombatTurnState.NonCombat;
        OnTurnChanged?.Invoke(CombatTurnState.NonCombat);
    }
}
