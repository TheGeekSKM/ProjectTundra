using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityMovement : MonoBehaviour
{
    [SerializeField] int _movementCost;
    public int MovementCost => _movementCost;

    PlayerStatsData _playerStatsData;
    CombatManager _combatManager;
    Rigidbody2D _rb;

    void OnValidate()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsData = GetComponent<EntityStatsContainer>().PlayerStatsData;
    }


    /// <summary>
    ///  Move the entity in the specified direction
    /// </summary>
    /// <param name="direction">Input a Vector2 Direction that you want the entity to move</param>
    public void Move(Vector2 direction)
    {
        if (_playerStatsData.CurrentActionPoints <= 0) return;

        direction.Normalize();
        _rb.MovePosition(_rb.position + direction);

        // Subtract the movement cost from the player's action points
        _playerStatsData.CurrentActionPoints -= _movementCost;
		Debug.Log("Subtracted " + _movementCost + " movement point from total action points, resulting in " + _playerStatsData.CurrentActionPoints + " total points!");
    }



    [ContextMenu("Move Up")]
    public void MoveUp()
    {
        Move(Vector2.up);
    }

    [ContextMenu("Move Down")]
    public void MoveDown()
    {
        Move(Vector2.down);
    }

    [ContextMenu("Move Left")]
    public void MoveLeft()
    {
        Move(Vector2.left);
    }

    [ContextMenu("Move Right")]
    public void MoveRight()
    {
        Move(Vector2.right);
    }
}
