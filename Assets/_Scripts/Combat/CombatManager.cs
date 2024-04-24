using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CombatTurnState
{
    NonCombat,
    Player,
    Enemy,
    Room,
    Win,
    Lose,
	CameraMove

}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    [SerializeField] CombatFSM combatFSM;

    [SerializeField] private List<EnemyBrain> _enemies;
    public List<EnemyBrain> Enemies => _enemies;

    [SerializeField] CombatTurnState _currentTurnState;
	public event System.Action<CombatTurnState> OnTurnChanged;
    [SerializeField] private EntityStamina _playerStamina;

    [SerializeField] RectTransform _playerControlsPanel;
    float _playerControlsPanelYPos;
    [SerializeField] RectTransform _movementControlsPanel;
    float _movementControlsPanelYPos;

    int _currentEnemyIndex = 0;

    void Awake()
    {
        if (combatFSM == null) combatFSM = GetComponent<CombatFSM>();
        if (combatFSM == null) combatFSM = gameObject.AddComponent<CombatFSM>();
		if (_playerStamina == null) _playerStamina = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStamina>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

		//save starting controls positions
		_playerControlsPanelYPos = _playerControlsPanel.anchoredPosition.y;
		_movementControlsPanelYPos = _movementControlsPanel.anchoredPosition.y;
	}

    void OnEnable()
    {
        _playerStamina.OnActionPointsChanged += HandlePlayerStatsChange;
    }

    void OnDisable()
    {
        _playerStamina.OnActionPointsChanged -= HandlePlayerStatsChange;
    }

    void Start()
    {
		if (_currentTurnState == CombatTurnState.NonCombat)
			combatFSM.ChangeState(combatFSM.NonCombatState);
	}

    // Check if player has enough action points, and if they don't, switch to enemy turn
    void HandlePlayerStatsChange()
    {
        if (_playerStamina.CurrentActionPoints <= 0)
        {
            combatFSM.ChangeState(combatFSM.EnemyCombatState);
        }
    }


    public void AddEnemy(EnemyBrain enemy)
    {
        _enemies.Add(enemy);
        enemy.OnEnemyTurnEnded += EnemyEntityTurnEnded;

        combatFSM.ChangeState(combatFSM.PlayerCombatState);
    }

    public void RemoveEnemy(EnemyBrain enemy)
    {
        enemy.OnEnemyTurnEnded -= EnemyEntityTurnEnded;
        _enemies.Remove(enemy);
        
        if (WinCheck()) return;
    }


    // starts off the enemy turns by calling the first enemy in the list and starting their turn
    void StartEnemiesTurn()
    {
        if (_enemies.Count > 0)
        {
            _enemies[0].StartEnemyTurn();
        }
    }

    void EnemyEntityTurnEnded(string enemyName)
    {
        Debug.Log($"Enemy {enemyName} turn ended");

        // if there are no more enemies, switch to player turn
        if (_currentEnemyIndex >= _enemies.Count - 1)
        {
            _currentEnemyIndex = 0;
            Debug.Log("All enemies have taken their turn");
            combatFSM.ChangeState(combatFSM.PlayerCombatState);
        }
        else // if there are more enemies, start the next enemy's turn
        {
            if (WinCheck()) return;

            _currentEnemyIndex++;
            _enemies[_currentEnemyIndex].StartEnemyTurn();
        }

    }

	public void CameraMoving(bool toggle)
	{
		if (toggle)
			combatFSM.ChangeState(combatFSM.CameraMoveState);
		else if (!toggle)
		{
			//ENTER ROOM LOGIC MIGHT GO HERE?
			combatFSM.ChangeState(combatFSM.NonCombatState);
		}
	}


    void FireEvent()
    {
        OnTurnChanged?.Invoke(_currentTurnState);
    }

    bool WinCheck()
    {
        if (_enemies.Count <= 0)
        {   
            combatFSM.ChangeState(combatFSM.NonCombatState);
            _currentTurnState = CombatTurnState.NonCombat;

            FireEvent();
            return true;
        }
        return false;
    }

    public void PlayerTurnIntro()
    {
        _currentTurnState = CombatTurnState.Player;
        FireEvent();
    }

    public void AnimatePlayerControlsIntro()
    {
        _playerControlsPanel.DOAnchorPosY(0, 0.5f);
    }

    public void AnimatePlayerControlsOutro()
    {
        _playerControlsPanel.DOAnchorPosY(_playerControlsPanelYPos, 0.5f);
    }

    public void AnimatePlayerMovementControlsIntro()
    {
        _movementControlsPanel.DOAnchorPosY(0, 0.5f);
    }

    public void AnimatePlayerMovementControlsOutro()
    {
        _movementControlsPanel.DOAnchorPosY(_movementControlsPanelYPos, 0.5f);
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

	public void CameraMove()
	{
		_currentTurnState = CombatTurnState.CameraMove;
		FireEvent();
	}
}
