using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityMovement : MonoBehaviour
{
    [SerializeField] int _movementLeft;
    public int MovementLeft => _movementLeft;

    PlayerStatsData _playerStatsData;
    CombatManager _combatManager;
    Rigidbody2D _rb;

    void OnValidate()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsData = GetComponent<EntityStatsContainer>().PlayerStatsData;
    }

    void Awake()
    {
        _playerStatsData.OnTotalActionPointsChanged += CheckTotalMovementSpeed;
    }

    void OnEnable()
    {
        _combatManager = CombatManager.Instance;
        _combatManager.OnTurnChanged += HandleTurnChange;
    }

    void Start()
    {
        CheckTotalMovementSpeed();
    }

    void OnDisable()
    {
        _playerStatsData.OnTotalActionPointsChanged -= CheckTotalMovementSpeed;
        _combatManager.OnTurnChanged -= HandleTurnChange;
    }

    void HandleTurnChange(CombatTurnState turnState)
    {
        switch (turnState)
        {
            // Reset movement when it's the player's turn
            case CombatTurnState.Player:
                CheckTotalMovementSpeed();
                break;
            
            // player can't move when it's the enemy's turn
            case CombatTurnState.Enemy:
                _movementLeft = 0;
                break;

            // player's movement is not counted when it's not in combat
            case CombatTurnState.NonCombat:
                DisableMovementCounter();
                break;
        }
    }

    // Disable movement counter when player is not in combat
    void DisableMovementCounter()
    {
        _movementLeft = -1;
    }

    // Enable movement counter when player is in combat
    private void CheckTotalMovementSpeed()
    {
        // Reset movement counter to total action points remaining
        _movementLeft = _playerStatsData.CurrentActionPoints;
    }

    /// <summary>
    ///  Move the entity in the specified direction
    /// </summary>
    /// <param name="direction">Input a Vector2 Direction that you want the entity to move</param>
    public void Move(Vector2 direction)
    {
        Debug.Log("Moving");
        if (_movementLeft == 0)
        {
            Debug.Log("No movement left");
            return;
        }

        if (_movementLeft > 0) 
        {
            _movementLeft--;
            _playerStatsData.CurrentActionPoints--;
        }
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
