using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon Item")]
public class WeaponItemData : BaseItemData
{
    [SerializeField] private int _damageBonus;
    public int DamageBonus => _damageBonus;

    [SerializeField] private DamageType _damageType;
    public DamageType DamageType => _damageType;

    [SerializeField] private int _attackRange;
    public int AttackRange => _attackRange;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Dealt {_damageBonus} {_damageType} damage with {ItemName} at a range of {_attackRange} units.");
    }
}

public enum DamageType
{
    Physical,
    Magical
}
