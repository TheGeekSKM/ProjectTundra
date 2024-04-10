using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] EntityStatsContainer _playerStats;
    public EntityStatsContainer PlayerStats => _playerStats;
    [SerializeField] EntityMovement _playerMovement;
    public EntityMovement PlayerMovement => _playerMovement;
    [SerializeField] EntityAttackManager _playerAttackManager;
    public EntityAttackManager PlayerAttackManager => _playerAttackManager;
    [SerializeField] PlayerInputBrain _playerInputBrain;
    public PlayerInputBrain PlayerInputBrain => _playerInputBrain;

    void OnValidate()
    {
        if (_playerStats == null)
        {
            _playerStats = GetComponent<EntityStatsContainer>();
        }
        if (_playerMovement == null)
        {
            _playerMovement = GetComponent<EntityMovement>();
        }
        if (_playerAttackManager == null)
        {
            _playerAttackManager = GetComponent<EntityAttackManager>();
        }
        if (_playerInputBrain == null)
        {
            _playerInputBrain = GetComponent<PlayerInputBrain>();
        }
    }
}
