using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] private int _moveSpeed = 1;
    public int MoveSpeed => _moveSpeed;

    public void SetMoveSpeed(int moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void Move(Vector2 direction)
    {
        transform.Translate(direction * _moveSpeed);
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
