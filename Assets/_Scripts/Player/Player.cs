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

    void OnEnable()
    {
        _playerHealth.OnDeath += OnDeath;
        _playerAttackManager.OnAttackWithoutWeapon += NoWeaponNotify;
        CombatManager.Instance.OnEnemyTargeted += TargetEnemy;
    }
     
    void OnDisable()
    {
        _playerHealth.OnDeath -= OnDeath;
        _playerAttackManager.OnAttackWithoutWeapon -= NoWeaponNotify;
        CombatManager.Instance.OnEnemyTargeted -= TargetEnemy;
    }

    void HandleReferences()
    {
        _playerStats = GetComponent<EntityStatsContainer>();
        _playerMovement = GetComponent<EntityMovement>();
        _playerAttackManager = GetComponent<EntityAttackManager>();
        _playerInputBrain = GetComponent<PlayerInputBrain>();
        _playerHealth = GetComponent<EntityHealth>();
        _playerStamina = GetComponent<EntityStamina>();
        _playerAnimationController = GetComponentInChildren<EntityAnimationController>();
        _playerInventoryManager = GetComponent<EntityInventoryManager>();
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
    [SerializeField] EntityAnimationController _playerAnimationController;
    public EntityAnimationController PlayerAnimationController => _playerAnimationController;
    [SerializeField] EntityInventoryManager _playerInventoryManager;
    public EntityInventoryManager PlayerInventoryManager => _playerInventoryManager;

    public System.Action OnPlayerInitialize;

    void TargetEnemy()
    {
        // point the attack origin at the targeted enemy
        PlayerAttackManager.AttackOrigin.right = (CombatManager.Instance.TargetedEnemy.transform.position - PlayerAttackManager.AttackOrigin.position).normalized;
    }

    void OnDeath()
    {
        Debug.LogError("Player Died");
        var sceneFSM = SceneController.Instance.SceneFSM;
        sceneFSM.ChangeState(sceneFSM.LoseMenuState);
    }

    void NoWeaponNotify()
    {
        // let the player know that there isn't a weapon in their inventory...
        NotificationManager.Instance.Notify(
            new NotificationData("You don't have a weapon in your inventory...", "NO WEAPON FOUND", 2.5f, ENotificationType.Warning)
        );
    }

    public void Initialize()
    {
        _playerAnimationController.Initialize();
        _playerHealth.Initialize();
        _playerStamina.Initialize();
        _playerInventoryManager.Initialize();
        OnPlayerInitialize?.Invoke();
    }


}
