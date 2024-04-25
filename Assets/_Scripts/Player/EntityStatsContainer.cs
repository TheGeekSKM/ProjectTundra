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
    [SerializeField] SpriteRenderer _spriteRenderer;

    public void SetPlayerStatsData(PlayerStatsData playerStatsData)
    {
        _playerStatsData = playerStatsData;
        if (_spriteRenderer) _spriteRenderer.sprite = playerStatsData.EntitySprite;
    }


    void Awake()
    {
        if (!_spriteRenderer) _spriteRenderer = GetComponent<SpriteRenderer>();
        if (!_spriteRenderer) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (_playerStatsData.EntityType == EntityType.Enemy)
        {
            _spriteRenderer.sprite = _playerStatsData.EntitySprite;
        }
    }

}
