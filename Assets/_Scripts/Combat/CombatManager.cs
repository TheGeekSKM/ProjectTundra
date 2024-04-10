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

    [SerializeField] private List<EnemyBrain> _enemies;
    public List<EnemyBrain> Enemies => _enemies;

    [SerializeField] CombatTurnState _currentTurnState;
    public event System.Action<CombatTurnState> OnTurnChanged;
    [SerializeField] private PlayerStatsData _playerStatsData;

    int _currentEnemyIndex = 0;

    void OnValidate()
    {
        if (combatFSM == null) combatFSM = GetComponent<CombatFSM>();
        if (combatFSM == null) combatFSM = gameObject.AddComponent<CombatFSM>();
    }

    void OnEnable()
    {
        _playerStatsData.OnCurrentActionPointsChanged += HandlePlayerStatsChange;
    }

    void OnDisable()
    {
        _playerStatsData.OnCurrentActionPointsChanged -= HandlePlayerStatsChange;
    }

    void Start()
    {
        combatFSM.ChangeState(combatFSM.NonCombatState);
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

    public void AddEnemy(EnemyBrain enemy)
    {
        _enemies.Add(enemy);
        enemy.OnEnemyTurnEnded += EnemyTurnEnded;

        combatFSM.ChangeState(combatFSM.PlayerCombatState);
    }

    public void RemoveEnemy(EnemyBrain enemy)
    {
        enemy.OnEnemyTurnEnded -= EnemyTurnEnded;
        _enemies.Remove(enemy);
        
        WinCheck();
    }


    // starts off the enemy turns by calling the first enemy in the list and starting their turn
    void StartEnemiesTurn()
    {
        if (_enemies.Count > 0)
        {
            _enemies[0].StartEnemyTurn();
        }
    }

    void EnemyTurnEnded()
    {
        // if there are no more enemies, switch to player turn
        if (_currentEnemyIndex >= _enemies.Count - 1)
        {
            _currentEnemyIndex = 0;
            combatFSM.ChangeState(combatFSM.PlayerCombatState);
        }
        else // if there are more enemies, start the next enemy's turn
        {
            _currentEnemyIndex++;
            _enemies[_currentEnemyIndex].StartEnemyTurn();
        }

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
        StartEnemiesTurn();
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
