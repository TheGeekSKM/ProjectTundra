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
        else _targetImage.SetActive(true);
    }

    void TouchPerformed(Vector2 position)
    {
        if (CombatManager.Instance.CurrentTurnState != CombatTurnState.Player) return;

        var enemy = GetEnemyAtPosition(position);
        if (enemy == null) return;

        CombatManager.Instance.SetTargetedEnemy(enemy);
        _targetImage.transform.position = enemy.transform.position;
    }

    EnemyBrain GetEnemyAtPosition(Vector2 position)
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(position);
        var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider == null) return null;

        var enemy = hit.collider.GetComponent<EnemyBrain>();
        return enemy;
    }
}
