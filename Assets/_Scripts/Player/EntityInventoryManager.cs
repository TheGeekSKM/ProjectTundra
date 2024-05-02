using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInventoryManager : MonoBehaviour
{
    [SerializeField] EntityStatsContainer _entityStatsContainer;
    public EntityStatsContainer EntityStatsContainer {
        get
        {
            if (!_entityStatsContainer)
            {
                _entityStatsContainer = GetComponent<EntityStatsContainer>();
            }
            return _entityStatsContainer;
        }
        set
        {
            _entityStatsContainer = value;
            Debug.Log($"EntityStatsContainer set to: {_entityStatsContainer}");
        }
    }
    [SerializeField] ItemContainer _entityInventory;
    public ItemContainer EntityInventory => _entityInventory;


    public void Initialize()
    {
        while (!EntityStatsContainer)
        {
            EntityStatsContainer = GetComponent<EntityStatsContainer>();
        }

        _entityInventory = new ItemContainer();
        var listOfItems = EntityStatsContainer.PlayerStatsData.ItemContainer.GetItems();
        
        foreach (var item in listOfItems)
        {
            Debug.Log($"Item: {item.ItemName}");
        }

        if (listOfItems.Count > 0)
        {
            foreach (var item in EntityStatsContainer.PlayerStatsData.ItemContainer.GetItems())
            {
                _entityInventory.AddItem(item);
                Debug.Log($"Added item: {item.ItemName}");
            }
        }

        if (EntityStatsContainer.PlayerStatsData.PossibleRandomItems.Length <= 0) return;
        int randomItemIndex = Random.Range(0, EntityStatsContainer.PlayerStatsData.PossibleRandomItems.Length - 1);
        BaseItemData randomItem = EntityStatsContainer.PlayerStatsData.PossibleRandomItems[randomItemIndex];
        _entityInventory.AddItem(randomItem);
        Debug.Log($"Added random item: {randomItem.ItemName}");
        Debug.Log($"{EntityInventory.ContainerName} now has {EntityInventory.GetItems().Count} items.");
        foreach (var item in EntityInventory.GetItems())
        {
            Debug.Log($"Item: {item.ItemName}");
        }
    }
}
