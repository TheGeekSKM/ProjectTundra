using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
public class ConsumableItemData : BaseItemData
{
    [SerializeField] private int _amountPerUse;
    public int AmountPerUse => _amountPerUse;

    [SerializeField] private EStatType _statType;
    public EStatType StatType => _statType;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Consumed {_amountPerUse} of {ItemName}");
    }

    
    
}
