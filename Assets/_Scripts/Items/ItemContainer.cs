using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer
{
    [SerializeField] List<BaseItemData> Items;
    public List<BaseItemData> GetItems() => Items;

    public event System.Action<BaseItemData> OnItemContainerChanged;

    public void AddItem(BaseItemData item)
    {
        foreach (var i in Items)
        {
            if (i is WeaponItemData)
            {
                int indexOfWeapon = Items.IndexOf(i);
                SetItem(indexOfWeapon, item);
            }
        }
        Items.Add(item);
        OnItemContainerChanged?.Invoke(item);
    }
    public void RemoveItem(BaseItemData item)
    {
        Items.Remove(item);
        OnItemContainerChanged?.Invoke(item);
    }
    public void SetItem(int index, BaseItemData item)
    {
        Items[index] = item;
        OnItemContainerChanged?.Invoke(item);
    }

    public WeaponItemData GetWeapon()
    {
        foreach (var item in Items)
        {
            if (item is WeaponItemData)
            {
                return (WeaponItemData)item;
            }
        }
        return null;
    }
}