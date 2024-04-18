using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStatsContainer))]
public class EntityAttackManager : MonoBehaviour
{
    private EntityStatsContainer _entityStatsContainer;
    private EntityStamina _entityStamina;
    [SerializeField] Transform _attackPoint;
    public Transform AttackPoint => _attackPoint;

    void Awake()
    {
        _entityStatsContainer = GetComponent<EntityStatsContainer>();
        _entityStamina = GetComponent<EntityStamina>();
    }
    // void OnEnable()
    // {
    //     CombatManager.Instance.OnTurnChanged += HandleTurnChange;
    // }

    // void OnDisable()
    // {
    //     CombatManager.Instance.OnTurnChanged -= HandleTurnChange;
    // }

    // void HandleTurnChange(CombatTurnState turnState)
    // {
    //     Debug.Log("Turn changed to " + turnState);
    //     switch (turnState)
    //     {
    //         case CombatTurnState.Player:
    //             _ignoreInput = false;
    //             break;
    //         case CombatTurnState.Enemy:
    //             //_entityStatsContainer.PlayerStatsData.CurrentActionPoints = 0;
    //             _ignoreInput = true;
    //             break;
    //         case CombatTurnState.NonCombat:
    //             _ignoreInput = true;
    //             break;
    //     }
    // }

    [ContextMenu("Attack")]
    public void Attack()
    {

        // get the weapon from the player's inventory
        var weapon = _entityStatsContainer.ItemContainer.GetWeapon();

        // if player has no action points, player cannot attack
        if (_entityStamina.CurrentActionPoints <= 0) return;
        
        // if weapon is not equipped, player cannot attack
        if (!HandleWeaponChecks(weapon)) return;

        // subtract the action points from the player
        SubtractAP(weapon.APCost);

        // initialize the attack object
        InitializeAttackObject(weapon);
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
                _entityStatsContainer.EntityType);
        }
        else if (weapon.AttackType == AttackType.Ranged)
        {
            Debug.Log("Ranged attack");
            attackObject.Initialize(20f, 3f, 0.2f, _entityStatsContainer.PlayerStatsData.Damage + weapon.DamageBonus, weapon, _entityStatsContainer.EntityType);
        }
    }

}
