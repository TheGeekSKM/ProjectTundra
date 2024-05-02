using System.Net.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "PlayerStats/PlayerStatsData")]
public class PlayerStatsData : ScriptableObject
{
    [Header("Basic Entity Stats")]
    [SerializeField] private int _totalActionPoints;
    public int TotalActionPoints
    {
        get
        {
            if (_entityType == EntityType.Enemy)
            {
                return _totalActionPoints + GameDataManager.Instance.EnemyDifficulty;
            }
            return _totalActionPoints;
        }
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
        get
        {
            if (_entityType == EntityType.Enemy)
            {
                return _maxHealth + GameDataManager.Instance.EnemyDifficulty;
            }
            return _maxHealth;
        }
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
        get
        {
            if (_entityType == EntityType.Enemy)
            {
                return _damage + GameDataManager.Instance.EnemyDifficulty;
            }
            return _damage;
        }
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
        get
        {
            if (_entityType == EntityType.Enemy)
            {
                return _aOERange + GameDataManager.Instance.EnemyDifficulty;
            }
            return _aOERange;
        }
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

    [SerializeField] private PlayerClass _playerClass;
    public PlayerClass PlayerClass
    {
        get => _playerClass;
        set
        {
            _playerClass = value;
            OnPlayerClassChanged?.Invoke();
        }
    }
    public event Action OnPlayerClassChanged;

    [SerializeField] private EnemyClass _enemyClass;
    public EnemyClass EnemyClass
    {
        get => _enemyClass;
        set
        {
            _enemyClass = value;
            OnEnemyClassChanged?.Invoke();
        }
    }
    public event Action OnEnemyClassChanged;

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
    [SerializeField] private int _healCost = 1;
    public int HealCost => _healCost;

    [Header("Entity Types")]
    [SerializeField] private AttackType _entityAttackType;
    public AttackType EntityAttackType => _entityAttackType;
    [SerializeField] private EntityType _entityType;
    public EntityType EntityType => _entityType;

    [Header("Inventory")]
    [SerializeField] private ItemContainer _itemContainer;
    public ItemContainer ItemContainer => _itemContainer;
    [SerializeField] private BaseItemData[] _possibleRandomItems;
    public BaseItemData[] PossibleRandomItems => _possibleRandomItems; 

    [Header("Art")]
    [SerializeField] private Sprite _sprite;
    public Sprite EntitySprite => _sprite;
    [SerializeField] private Sprite _deathSprite;
    public Sprite DeathSprite => _deathSprite;
    [SerializeField] private RuntimeAnimatorController _animatorController;
    public RuntimeAnimatorController AnimatorController => _animatorController;

    public WeaponItemData EquippedWeapon {
        get
        {
            foreach (var item in _itemContainer.GetItems())
            {
                if (item is WeaponItemData weapon)
                {
                    return weapon;
                }
            }
            return null;
        }
    
    }
}

public enum PlayerClass
{
    Ranger,
    Scout,
    Mage
}

public enum EnemyClass
{
    Slime,
    Skeleton,
    Bat
}
