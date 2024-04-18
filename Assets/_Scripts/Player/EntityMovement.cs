using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityMovement : MonoBehaviour
{
    private EntityStamina _entityStamina;

    PlayerStatsData _playerStatsData;
    Rigidbody2D _rb;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsData = GetComponent<EntityStatsContainer>().PlayerStatsData;
        _entityStamina = GetComponent<EntityStamina>();
    }


    /// <summary>
    ///  Move the entity in the specified direction
    /// </summary>
    /// <param name="direction">Input a Vector2 Direction that you want the entity to move</param>
    public void Move(Vector2 direction)
    {
        if (_entityStamina.CurrentActionPoints <= 0) return;

        direction.Normalize();
        _rb.MovePosition(_rb.position + direction);
		// Debug.Log("Subtracted " + _movementCost + " movement point from total action points, resulting in " + _playerStatsData.CurrentActionPoints + " total points!");
    }

    void SubtractAP()
    {
        _entityStamina.SubtractAP(_playerStatsData.MovementCost);
    }

    [ContextMenu("Move Up")]
    public void MoveUp(bool subtractAP)
    {
        Move(Vector2.up);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Down")]
    public void MoveDown(bool subtractAP)
    {
        Move(Vector2.down);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Left")]
    public void MoveLeft(bool subtractAP)
    {
        Move(Vector2.left);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Right")]
    public void MoveRight(bool subtractAP)
    {
        Move(Vector2.right);
        if (subtractAP) SubtractAP();
    }
}
