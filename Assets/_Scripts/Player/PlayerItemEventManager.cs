using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemEventManager : MonoBehaviour
{
    [SerializeField] ConsumableItemData[] _allPossibleBuffingItems;
    [SerializeField] EntityStatsContainer _playerStats;

    private void OnEnable()
    {
        foreach (var item in _allPossibleBuffingItems)
        {
            item.Subscribe(BuffPlayer);
        }
    }

    private void OnDisable()
    {
        foreach (var item in _allPossibleBuffingItems)
        {
            item.Unsubscribe(BuffPlayer);
        }
    }

    void BuffPlayer(BaseItemData item)
    {
        if (item is ConsumableItemData consumableItem)
        {
            switch (consumableItem.StatType)
            {
                case EStatType.Health:
                    _playerStats.PlayerStatsData.MaxHealth += consumableItem.AmountPerUse;
                    break;
                case EStatType.TotalAP:
                    _playerStats.PlayerStatsData.TotalActionPoints += consumableItem.AmountPerUse;
                    break;
                case EStatType.Damage:
                    _playerStats.PlayerStatsData.Damage += consumableItem.AmountPerUse;
                    break;
                case EStatType.AOERange:
                    _playerStats.PlayerStatsData.AOERange += consumableItem.AmountPerUse;
                    break;
                default:
                    break;
            }
        }
    }
}
