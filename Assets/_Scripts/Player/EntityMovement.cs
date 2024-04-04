using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityMovement : MonoBehaviour
{

    [SerializeField] int _movementLeft;
    public int MovementLeft => _movementLeft;

    private int _totalMovementSpeed;
    public int TotalMoveSpeed => _totalMovementSpeed;

    PlayerStatsData _playerStatsData;
    Rigidbody2D _rb;

    void OnValidate()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsData = GetComponent<EntityStatsContainer>().PlayerStatsData;
    }

    void Awake()
    {
        _playerStatsData.OnMovementSpeedChanged += CheckTotalMovementSpeed;
    }

    private void Start()
    {
        CheckTotalMovementSpeed();
    }

    private void CheckTotalMovementSpeed()
    {
        _totalMovementSpeed = _playerStatsData.TotalMovementSpeed;
        Debug.Log($"Total Movement Speed: {_totalMovementSpeed}");
        _movementLeft = _totalMovementSpeed;
    }

    public void Move(Vector2 direction)
    {
        Debug.Log("Moving");
        if (_movementLeft <= 0)
        {
            Debug.Log("No movement left");
            return;
        }

        _movementLeft--;
        direction.Normalize();
        _rb.MovePosition(_rb.position + direction);
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
