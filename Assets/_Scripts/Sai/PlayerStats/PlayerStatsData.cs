using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "PlayerStats/PlayerStatsData")]
public class PlayerStatsData : ScriptableObject
{
    [Header("Visible Stats")]
    [SerializeField] private int _totalActionPoints;
    public int TotalActionPoints
    {
        get => _totalActionPoints;
        set
        {
            _totalActionPoints = value;
            OnTotalActionPointsChanged?.Invoke();
        }
    }
    public event Action OnTotalActionPointsChanged;

    private int _currentActionPoints;
    public int CurrentActionPoints
    {
        get => _currentActionPoints;
        set
        {
            _currentActionPoints = value;
            OnCurrentActionPointsChanged?.Invoke();
        }
    }
    public event Action OnCurrentActionPointsChanged;
    
    
    [SerializeField] private int _totalHealth;
    public int TotalHealth
    {
        get => _totalHealth;
        set
        {
            _totalHealth = value;
            OnTotalHealthChanged?.Invoke();
        }
    }
    public event Action OnTotalHealthChanged;
    
    
    [SerializeField] private int _damage;
    public int Damage
    {
        get => _damage;
        set
        {
            _damage = value;
            OnDamageChanged?.Invoke();
        }
    }
    public event Action OnDamageChanged;
    
    
    [SerializeField] private int _aOERange;
    public int AOERange
    {
        get => _aOERange;
        set
        {
            _aOERange = value;
            OnAOERangeChanged?.Invoke();
        }
    }
    public event Action OnAOERangeChanged;
    


    [Header("Hidden Stats")]
    [SerializeField] private int _armor;
    public int Armor
    {
        get => _armor;
        set
        {
            _armor = value;
            OnArmorChanged?.Invoke();
        }
    }
    public event Action OnArmorChanged;


    public void ResetActionPoints()
    {
        CurrentActionPoints = TotalActionPoints;
    }

}
