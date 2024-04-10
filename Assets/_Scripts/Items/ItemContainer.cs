using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer
{
    [SerializeField] private string _containerName;
    public string ContainerName => _containerName;
    [SerializeField] List<BaseItemData> Items;

    /// <summary>
    /// Gets the list of items in the container.
    /// </summary>
    /// <returns>The list of items.</returns>
    public List<BaseItemData> GetItems() => Items;

    public event System.Action<BaseItemData> OnItemContainerChanged;

    /// <summary>
    /// Adds an item to the container.
    /// </summary>
    /// <param name="item">The item to add.</param>
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

    /// <summary>
    /// Removes an item from the container.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    public void RemoveItem(BaseItemData item)
    {
        Items.Remove(item);
        OnItemContainerChanged?.Invoke(item);
    }

    /// <summary>
    /// Sets an item at the specified index in the container.
    /// </summary>
    /// <param name="index">The index of the item.</param>
    /// <param name="item">The item to set.</param>
    public void SetItem(int index, BaseItemData item)
    {
        Items[index] = item;
        OnItemContainerChanged?.Invoke(item);
    }

    /// <summary>
    /// Gets the weapon item from the container.
    /// </summary>
    /// <returns>The weapon item, or null if no weapon item is found.</returns>
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