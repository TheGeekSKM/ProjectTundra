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
    public void SetPlayerStatsData(PlayerStatsData playerStatsData)
    {
        _playerStatsData = playerStatsData;
    }
}
