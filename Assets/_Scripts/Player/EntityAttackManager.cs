using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(EntityStatsContainer))]
public class EntityAttackManager : MonoBehaviour
{
    private EntityStatsContainer _entityStatsContainer;
    [SerializeField] Transform _attackPoint;

    void OnValidate()
    {
        _entityStatsContainer = GetComponent<EntityStatsContainer>();
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
        var weapon = _entityStatsContainer.ItemContainer.GetWeapon();
        if (weapon == null)
        {
            Debug.Log("No weapon equipped");
            return;
        }

        if (weapon.AttackPrefab == null)
        {
            Debug.Log("No attack prefab");
            return;
        }

        var attackObject = Instantiate(weapon.AttackPrefab, _attackPoint.position, _attackPoint.rotation).GetComponent<AttackObjectController>();

        if (weapon.AttackType == AttackType.Melee)
        {
            attackObject.Initialize(0f, 0.5f, 
                _entityStatsContainer.PlayerStatsData.AOERange, 
                _entityStatsContainer.PlayerStatsData.Damage + weapon.DamageBonus,
                weapon, 
                _entityStatsContainer.EntityType);
        }
        else if (weapon.AttackType == AttackType.Ranged)
        {
            attackObject.Initialize(20f, 3f, 0.2f, _entityStatsContainer.PlayerStatsData.Damage + weapon.DamageBonus, weapon, _entityStatsContainer.EntityType);
        }

        weapon.Use();
    }
}
