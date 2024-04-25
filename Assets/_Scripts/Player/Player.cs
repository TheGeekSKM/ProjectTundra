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

        HandleReferences();
    }

    void Start()
    {
        _playerHealth.OnDeath += OnDeath;
    }

    void OnDisable()
    {
        _playerHealth.OnDeath -= OnDeath;
    }

    void HandleReferences()
    {
        if (_playerStats == null) _playerStats = GetComponent<EntityStatsContainer>();
        if (_playerMovement == null) _playerMovement = GetComponent<EntityMovement>();
        if (_playerAttackManager == null) _playerAttackManager = GetComponent<EntityAttackManager>();
        if (_playerInputBrain == null) _playerInputBrain = GetComponent<PlayerInputBrain>();
        if (_playerHealth == null) _playerHealth = GetComponent<EntityHealth>();
        if (_playerStamina == null) _playerStamina = GetComponent<EntityStamina>();
    }

    [SerializeField] EntityStatsContainer _playerStats;
    public EntityStatsContainer PlayerStats => _playerStats;
    [SerializeField] EntityMovement _playerMovement;
    public EntityMovement PlayerMovement => _playerMovement;
    [SerializeField] EntityAttackManager _playerAttackManager;
    public EntityAttackManager PlayerAttackManager => _playerAttackManager;
    [SerializeField] EntityHealth _playerHealth;
    public EntityHealth PlayerHealth => _playerHealth;
    [SerializeField] EntityStamina _playerStamina;
    public EntityStamina PlayerStamina => _playerStamina;
    [SerializeField] PlayerInputBrain _playerInputBrain;
    public PlayerInputBrain PlayerInputBrain => _playerInputBrain;


    void OnDeath()
    {
        var sceneFSM = SceneController.Instance.SceneFSM;
        sceneFSM.ChangeState(sceneFSM.LoseMenuState);
    }


}
