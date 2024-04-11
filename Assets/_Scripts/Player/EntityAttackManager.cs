using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityAttackManager : MonoBehaviour
{
    private EntityStatsContainer _entityStatsContainer;
    [SerializeField] Transform _attackPoint;
    public Transform AttackPoint => _attackPoint;

    bool _ignoreInput = false;

    void OnValidate()
    {
        _entityStatsContainer = GetComponent<EntityStatsContainer>();
    }

    void OnEnable()
    {
        CombatManager.Instance.OnTurnChanged += HandleTurnChange;
    }

    void OnDisable()
    {
        CombatManager.Instance.OnTurnChanged -= HandleTurnChange;
    }

    void HandleTurnChange(CombatTurnState turnState)
    {
        switch (turnState)
        {
            case CombatTurnState.Player:
                _ignoreInput = false;
                _entityStatsContainer.PlayerStatsData.ResetActionPoints();
                break;
            case CombatTurnState.Enemy:
                //_entityStatsContainer.PlayerStatsData.CurrentActionPoints = 0;
                _ignoreInput = true;
                break;
            case CombatTurnState.NonCombat:
                _ignoreInput = true;
                break;
        }
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
        // if not in combat state, player cannot attack
        if (_ignoreInput) return;

        // if player has no action points, player cannot attack
        if (_entityStatsContainer.PlayerStatsData.CurrentActionPoints <= 0) return;

        // get the weapon from the player's inventory
        var weapon = _entityStatsContainer.ItemContainer.GetWeapon();
        if (weapon == null)
        {
            Debug.Log("No weapon equipped");
            return;
        }

        // check if the weapon has an attack prefab
        if (weapon.AttackPrefab == null)
        {
            Debug.Log("No attack prefab");
            return;
        }

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

        // reduce the player's action points
        _entityStatsContainer.PlayerStatsData.CurrentActionPoints -= weapon.APCost;
    }
}
