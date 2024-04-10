using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItemData : ScriptableObject
{
    [SerializeField] private string _itemName;
    public string ItemName => _itemName;

    [TextArea(3, 10)]
    [SerializeField] private string _description;
    public string Description => _description;

    [SerializeField] private int _durability = 1;
    public int Durability => _durability;

    public bool ItemBroken => _durability == 0;

    [SerializeField] protected int _apCost;
    public int APCost => _apCost;

    protected readonly List<Action> _useEffects = new List<Action>();

    public virtual int Use()
    {
        if (_durability == 0)
        {
            Broken();
            return _apCost;
        }

        _durability--;
        RaiseUseEffects();
        return _apCost;
    }

    public virtual void Broken()
    {
        Debug.Log("Item is broken");
    }

    void RaiseUseEffects()
    {
        foreach (var effect in _useEffects)
        {
            effect?.Invoke();
        }
    }

    public void Subscribe(Action effect)
    {
        _useEffects.Add(effect);
    }

    public void Unsubscribe(Action effect)
    {
        _useEffects.Remove(effect);
    }

}
