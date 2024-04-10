using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy
}

public class EntityStatsContainer : MonoBehaviour
{
    [SerializeField] private PlayerStatsData _playerStatsData;
    public PlayerStatsData PlayerStatsData => _playerStatsData;

    [SerializeField] private EntityType _entityType;
    public EntityType EntityType => _entityType;

    [SerializeField] private ItemContainer _itemContainer;
    public ItemContainer ItemContainer => _itemContainer;

    public System.Action OnDeath;
    public System.Action OnTurnEnded;
}
