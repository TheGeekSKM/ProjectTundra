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
    
    
    [SerializeField] private int _totalMovementSpeed;
    public int TotalMovementSpeed
    {
        get => _totalMovementSpeed;
        set
        {
            _totalMovementSpeed = value;
            OnMovementSpeedChanged?.Invoke();
        }
    }
    public event Action OnMovementSpeedChanged;

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

}
