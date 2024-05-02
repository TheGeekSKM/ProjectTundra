using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectionManager : MonoBehaviour
{

    [SerializeField] private GameObject _targetImage;
    void OnEnable()
    {
        TouchManager.Instance.OnTap += TouchPerformed;
        CombatManager.Instance.OnTurnChanged += HandleTurnChange;
    }

    void OnDisable()
    {
        TouchManager.Instance.OnTap -= TouchPerformed;
        CombatManager.Instance.OnTurnChanged -= HandleTurnChange;
    }

    // When the turn changes, show the target image if it's the player's turn
    private void HandleTurnChange(CombatTurnState state)
    {
		if (state != CombatTurnState.Player) _targetImage.SetActive(false);
		else
		{
			_targetImage.SetActive(true);
			CombatManager.Instance.SetTargetedEnemy(CombatManager.Instance.Enemies[0]);
			_targetImage.transform.position = CombatManager.Instance.Enemies[0].transform.position;
		}
    }

    void TouchPerformed(Vector2 position)
    {
        // If it's not the player's turn, return
        if (CombatManager.Instance.CurrentTurnState != CombatTurnState.Player) return;
        
        // If the player doesn't have a weapon or the weapon is melee, return
        var playerWeapon = Player.Instance.PlayerStats.PlayerStatsData.EquippedWeapon;
        if (playerWeapon == null && playerWeapon.AttackType == AttackType.Melee) return;

        // Get the world position of the touch
        var worldPosition = Camera.main.ScreenToWorldPoint(position);
        
        // If the distance between the player and the touch position is greater than the player's attack range, notify player and return
        if (Vector3.Distance(worldPosition, Player.Instance.transform.position) > playerWeapon.AttackRange) {
            NotificationManager.Instance.Notify(
                new NotificationData("The selected enemy is too far", "Too Far", 1f, ENotificationType.Warning)
            );
            return;
        }

        var enemy = GetEnemyAtPosition(worldPosition);
        if (enemy == null) return;

        CombatManager.Instance.SetTargetedEnemy(enemy);
        _targetImage.transform.position = enemy.transform.position;
    }

    EnemyBrain GetEnemyAtPosition(Vector2 worldPosition)
    {
        var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider == null) return null;

        var enemy = hit.collider.GetComponent<EnemyBrain>();
        return enemy;
    }
}
