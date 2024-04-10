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
    [SerializeField] private PlayerStatsData _playerStatsData;

    void OnValidate()
    {
        if (combatFSM == null) combatFSM = GetComponent<CombatFSM>();
        if (combatFSM == null) combatFSM = gameObject.AddComponent<CombatFSM>();
    }

    void OnEnable()
    {
        _playerStatsData.OnTotalActionPointsChanged += HandlePlayerStatsChange;
    }

    void OnDisable()
    {
        _playerStatsData.OnTotalActionPointsChanged -= HandlePlayerStatsChange;
    }

    // Check if player has enough action points, and if they don't, switch to enemy turn
    void HandlePlayerStatsChange()
    {
        if (_playerStatsData.CurrentActionPoints <= 0)
        {
            combatFSM.ChangeState(combatFSM.EnemyCombatState);
        }
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

    void FireEvent()
    {
        OnTurnChanged?.Invoke(_currentTurnState);
    }

    void WinCheck()
    {
        if (_enemies.Count <= 0)
        {
            _currentTurnState = CombatTurnState.Win;
            FireEvent();
        }
    }

    public void PlayerTurnIntro()
    {
        _currentTurnState = CombatTurnState.Player;
        FireEvent();
    }

    public void EnemyTurnIntro()
    {
        _currentTurnState = CombatTurnState.Enemy;
        FireEvent();
    }

    public void RoomTurnIntro()
    {
        _currentTurnState = CombatTurnState.Room;
        FireEvent();

    }

    public void LoseCombat()
    {
        _currentTurnState = CombatTurnState.Lose;
        FireEvent();

    }

    public void WinCombat()
    {
        _currentTurnState = CombatTurnState.Win;
        FireEvent();

    }

    public void NonCombat()
    {
        _currentTurnState = CombatTurnState.NonCombat;
        FireEvent();

    }
}
