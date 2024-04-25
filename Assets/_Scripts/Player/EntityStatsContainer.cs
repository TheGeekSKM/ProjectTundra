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

    [SerializeField] SpriteRenderer _spriteRenderer;

    void Awake()
    {
        if (!_spriteRenderer) _spriteRenderer = GetComponent<SpriteRenderer>();
        if (!_spriteRenderer) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }   

    void Start()
    {
        SetSprite();
    }

    public void SetSprite()
    {
        if (_playerStatsData != null && _playerStatsData.EntitySprite != null)
        {
            _spriteRenderer.sprite = _playerStatsData.EntitySprite;
        }
    }
}
