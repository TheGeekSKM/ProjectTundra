using System.Collections;
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
    [SerializeField] private EnemyBrain _targetedEnemy;
    public EnemyBrain TargetedEnemy => _targetedEnemy;

    [SerializeField] CombatTurnState _currentTurnState;
    public CombatTurnState CurrentTurnState => _currentTurnState;
	public event System.Action<CombatTurnState> OnTurnChanged;
    public event System.Action OnEnemyTargeted;
    [SerializeField] private EntityStamina _playerStamina;

    [SerializeField] RectTransform _playerControlsPanel;
    float _playerControlsPanelYPos;
    [SerializeField] RectTransform _movementControlsPanel;
    float _movementControlsPanelYPos;
    [SerializeField] RectTransform _inventoryButton;
    float _inventoryButtonXPos;
    [SerializeField] RectTransform _backButton;
    float _backButtonXPos;

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
		MazeGen.Instance.StartMaze();

		if (_currentTurnState == CombatTurnState.NonCombat)
			combatFSM.ChangeState(combatFSM.NonCombatState);

        _inventoryButtonXPos = _inventoryButton.anchoredPosition.x;
        _backButtonXPos = _backButton.anchoredPosition.x;
    }

    // Check if player has enough action points, and if they don't, switch to enemy turn
    void HandlePlayerStatsChange(int currentStamina)
    {
        if (currentStamina <= 0)
        {
            combatFSM.ChangeState(combatFSM.EnemyCombatState);
        }
    }

    public void SetTargetedEnemy(EnemyBrain enemy)
    {
        _targetedEnemy = enemy;
        OnEnemyTargeted?.Invoke();
    }

    public void AddEnemy(EnemyBrain enemy)
    {
        _enemies.Add(enemy);
        enemy.OnEnemyTurnEnded += EnemyEntityTurnEnded;

        //combatFSM.ChangeState(combatFSM.PlayerCombatState);
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

	public void CameraMoving(bool toggle, RoomController rm)
	{
		if (toggle)
			combatFSM.ChangeState(combatFSM.CameraMoveState);
		else if (!toggle)
		{
			if (_enemies.Count > 0)
			{
				combatFSM.ChangeState(combatFSM.PlayerCombatState);
				rm.LockDoors();
				MusicManager.Instance.SwapTrack(EAudioEvent.CombatBGM);
			}
			else
				combatFSM.ChangeState(combatFSM.NonCombatState);
		}
	}

	public void WinMovement()
	{
		combatFSM.ChangeState(combatFSM.WinCombatState);
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
        if (_currentTurnState != CombatTurnState.NonCombat)
        {
            _backButton.DOAnchorPosX(-50, 0.5f);
        }
    }

    public void AnimatePlayerMovementControlsOutro()
    {
        _movementControlsPanel.DOAnchorPosY(_movementControlsPanelYPos, 0.5f);
        _backButton.DOAnchorPosX(_backButtonXPos, 0.5f);
    }

    public void EnemyTurnIntro()
    {

		StartCoroutine(EnemyDelay());
		IEnumerator EnemyDelay()
		{
			yield return new WaitForSeconds(1f);
			_currentTurnState = CombatTurnState.Enemy;
			FireEvent();
			StartEnemiesTurn();
		}
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
        _inventoryButton.DOAnchorPosX(0, 0.5f);

        if (!MusicManager.Instance) return;
        if (!MusicManager.Instance.currentTrack)
        {
            MusicManager.Instance.SwapTrack(EAudioEvent.NonCombatBGM);
            return;
        }
        else if (MusicManager.Instance.currentTrack.audioEvent != EAudioEvent.NonCombatBGM)
        {
            MusicManager.Instance.SwapTrack(EAudioEvent.NonCombatBGM);
        }
        
    }

    public void NonCombatOutro()
    {   
        _inventoryButton.DOAnchorPosX(_inventoryButtonXPos, 0.5f);
    }

	public void CameraMove()
	{
		_currentTurnState = CombatTurnState.CameraMove;
		FireEvent();
	}
}
