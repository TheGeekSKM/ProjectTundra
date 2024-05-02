using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : Tile
{
    [Header("Chest Inventory")]
    [SerializeField] private ItemContainer _itemContainer;

    [Header("Random Items")]
    [SerializeField] private bool _useRandomItems = true;
    [SerializeField] private Vector2Int _additionalRandomItemsRange = new Vector2Int(1, 2);
    [SerializeField] private List<BaseItemData> _randomItems;

    [Header("Debug")]
    [SerializeField] bool _useable = false;
    public override void Highlight()
    {
        if (!_useable) return;

		base.Highlight();
		ChestViewManager.Instance.OpenChest(_itemContainer, Player.Instance.PlayerInventoryManager.EntityInventory);
    }

    public override void Deselect()
    {

		base.Deselect();
        ChestViewManager.Instance.CloseChest();
    }

    public void SetUseable(bool u) => _useable = u;

    public void Init(ItemContainer itemContainer, bool isOffset, bool useCustomSprite = false, Sprite sprite = null) 
    {
        base.Init(isOffset, useCustomSprite);

        // Set custom sprite if available
        if (useCustomSprite && sprite) _renderer.sprite = sprite;
        _itemContainer = itemContainer;

        HandleRandomItems();
    }

    void HandleRandomItems()
    {
        if (!_useRandomItems) return;
        if (_randomItems.Count == 0)
        { 
            Debug.LogError($"{_itemContainer.ContainerName} has no random items available!!");
            return;
        }

        var tempArray = _randomItems;

        var numOfItemsToUse = Random.Range(_additionalRandomItemsRange.x, _additionalRandomItemsRange.y);

        for (int i = 0; i < numOfItemsToUse - 1; i++)
        {
            var itemToAdd = tempArray[Random.Range(0, tempArray.Count - 1)];

            _itemContainer.AddItem(itemToAdd);
            tempArray.Remove(itemToAdd);
        }
    }
}
