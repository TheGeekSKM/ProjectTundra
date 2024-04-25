using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : Tile
{
    [SerializeField] private ItemContainer _itemContainer;
    [SerializeField] bool _useable = false;
    public override void Highlight()
    {
        if (!_useable) return;

		base.Highlight();
		ChestViewManager.Instance.OpenChest(_itemContainer, Player.Instance.PlayerStats.PlayerStatsData.ItemContainer);
    }

    public override void Deselect()
    {

		base.Deselect();
        ChestViewManager.Instance.CloseChest();
    }

    public void SetUseable(bool u)
    {
        _useable = u;
    }
}
