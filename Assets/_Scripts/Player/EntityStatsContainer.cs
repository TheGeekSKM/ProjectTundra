using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatsContainer : MonoBehaviour
{
    [SerializeField] private PlayerStatsData _playerStatsData;
    public PlayerStatsData PlayerStatsData => _playerStatsData;
}
