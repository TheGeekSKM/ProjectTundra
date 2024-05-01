using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStamina : MonoBehaviour
{
    [SerializeField] EntityStatsContainer _entityStatsContainer;
    PlayerStatsData _playerStatsData;
    [SerializeField] int _currentActionPoints;
    public int CurrentActionPoints => _currentActionPoints;
    public System.Action<int> OnActionPointsChanged;

    void Awake()
    {
        if (_entityStatsContainer == null) _entityStatsContainer = GetComponent<EntityStatsContainer>();
    }

    public void Initialize()
    {
        _playerStatsData = _entityStatsContainer.PlayerStatsData;
        ResetAP();

        _playerStatsData.OnTotalActionPointsChanged += ResetAP;
        CombatManager.Instance.OnTurnChanged += HandleTurnChange;
    }

    public void SubtractAP(int amount)
    {
        
        _currentActionPoints -= amount;
        OnActionPointsChanged?.Invoke(_currentActionPoints);
    }

    void HandleTurnChange(CombatTurnState turnState)
    {
        if (turnState == CombatTurnState.Player) ResetAP();
    }

    public void ResetAP()
    {
        _currentActionPoints = _playerStatsData.TotalActionPoints;
        OnActionPointsChanged?.Invoke(CurrentActionPoints);
    }
}
