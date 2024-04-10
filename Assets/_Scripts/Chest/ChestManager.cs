using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : Tile
{
    [SerializeField] private ItemContainer _itemContainer;
    public override void Highlight()
    {
        ChestViewManager.Instance.OpenChest(_itemContainer, Player.Instance.PlayerStats.ItemContainer);
    }

    public override void Deselect()
    {
        ChestViewManager.Instance.CloseChest();
    }
}
