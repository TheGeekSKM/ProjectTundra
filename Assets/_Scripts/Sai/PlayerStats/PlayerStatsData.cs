using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "PlayerStats/PlayerStatsData")]
public class PlayerStatsData : ScriptableObject
{
    [Header("Basic Entity Stats")]
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
    
    [SerializeField] private int _maxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            OnMaxHealthChanged?.Invoke();
        }
    }
    public event Action OnMaxHealthChanged;
    
    
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

    [Header("Visible Stats")]
    [SerializeField] private int _movementCost;
    public int MovementCost
    {
        get => _movementCost;
        set
        {
            _movementCost = value;
            OnMovementCostChanged?.Invoke();
        }
    }
    public event Action OnMovementCostChanged;

    [SerializeField] private AttackType _entityAttackType;
    public AttackType EntityAttackType => _entityAttackType;
}
