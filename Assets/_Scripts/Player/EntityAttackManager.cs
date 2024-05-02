using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(EntityStatsContainer))]
public class EntityAttackManager : MonoBehaviour
{
    private EntityStatsContainer _entityStatsContainer;
    private EntityStamina _entityStamina;
    private EntityInventoryManager _entityInventoryManager;
    [SerializeField] Transform _attackPoint;
    [SerializeField] Transform _attackOrigin;
    public Transform AttackOrigin => _attackOrigin ? _attackOrigin : _attackPoint;
    public System.Action OnAttackWithoutWeapon;

    void Awake()
    {
        _entityStatsContainer = GetComponent<EntityStatsContainer>();
        _entityStamina = GetComponent<EntityStamina>();
        _entityInventoryManager = GetComponent<EntityInventoryManager>();
    }
    

    [ContextMenu("Attack")]
    public void Attack()
    {
        HandleSound();

        // get the weapon from the player's inventory
        var weapon = _entityInventoryManager.EntityInventory.GetWeapon();

        // if player has no action points, player cannot attack
        if (_entityStamina.CurrentActionPoints <= 0) return;
        
        // if weapon is not equipped, player cannot attack
        if (!HandleWeaponChecks(weapon)) return;

        // subtract the action points from the player
        SubtractAP(weapon.APCost);

        // initialize the attack object
        InitializeAttackObject(weapon);
    }

    void HandleSound()
    {
        // play attack sound
        switch (_entityStatsContainer.PlayerStatsData.EntityType)
        {
            case EntityType.Player:
                AudioManager.Instance.PlayAudio3D(EAudioEvent.PlayerAttack, transform.position);
                break;
            case EntityType.Enemy:
                AudioManager.Instance.PlayAudio3D(EAudioEvent.EnemyAttack, transform.position);
                break;
        }
    }

    /// <summary>
    ///  Subtract the action points from the player
    /// </summary>
    /// <param name="cost">Cost of AP to subtract </param>
    void SubtractAP(int cost)
    {
        // reduce the player's action points
        _entityStamina.SubtractAP(cost);
    }

    /// <summary>
    ///  Check if the player can attack
    /// </summary>
    /// <param name="weapon"> Weapon to check </param> 
    /// <returns> True if the player can attack, false otherwise </returns> 
    bool HandleWeaponChecks(WeaponItemData weapon)
    {
        // check if the player has a weapon equipped
        if (weapon == null)
        {
            Debug.Log("No weapon equipped");
            OnAttackWithoutWeapon?.Invoke();
            return false;
        }

        // check if the weapon has an attack prefab
        if (weapon.AttackPrefab == null)
        {
            Debug.Log("No attack prefab");
            return false;
        }

        return true;
    }

    /// <summary>
    ///  Initialize the attack object
    /// </summary>
    /// <param name="weapon"> Weapon to initialize the attack object with </param>
    void InitializeAttackObject(WeaponItemData weapon)
    {
        // create the attack object
        var attackObject = Instantiate(weapon.AttackPrefab, _attackPoint.position, _attackPoint.rotation).GetComponent<AttackObjectController>();

        // initialize the attack object
        if (weapon.AttackType == AttackType.Melee)
        {
            Debug.Log("Melee attack");
            attackObject.Initialize(0f, 0.5f, 
                _entityStatsContainer.PlayerStatsData.AOERange, 
                _entityStatsContainer.PlayerStatsData.Damage + weapon.DamageBonus,
                weapon, 
                _entityStatsContainer.PlayerStatsData.EntityType,
                _attackOrigin.right);
        }
        else if (weapon.AttackType == AttackType.Ranged)
        {
            Debug.Log("Ranged attack");
            attackObject.Initialize(20f, 3f, 0.2f, 
                _entityStatsContainer.PlayerStatsData.Damage + weapon.DamageBonus, 
                weapon, 
                _entityStatsContainer.PlayerStatsData.EntityType, 
                _attackOrigin.right);
        }
    }

}
