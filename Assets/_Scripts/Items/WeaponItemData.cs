using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon Item")]
public class WeaponItemData : BaseItemData
{
    [SerializeField] private int _damageBonus;
    public int DamageBonus => _damageBonus;

    [SerializeField] private DamageType _damageType;
    public DamageType DamageType => _damageType;

    [SerializeField] private int _attackRange;
    public int AttackRange => _attackRange;

    [SerializeField] private GameObject _attackPrefab;
    public GameObject AttackPrefab => _attackPrefab;

    [SerializeField] private GameObject _hitEffectPrefab;
    public GameObject HitEffectPrefab => _hitEffectPrefab;

    [SerializeField] private AttackType _attackType;
    public AttackType AttackType => _attackType;

    public override int Use()
    {
        base.Use();
        if (ItemBroken)
        {
            return _apCost;
        }
        Debug.Log($"Dealt {_damageBonus} {_damageType} damage with {ItemName} at a range of {_attackRange} units.");
        return _apCost;
    }
}

public enum DamageType
{
    Physical,
    Magical
}
