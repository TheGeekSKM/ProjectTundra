using UnityEngine;

[RequireComponent(typeof(EntityMovement))]
public class PlayerInputBrain : MonoBehaviour
{
    [SerializeField] private EntityMovement _entityMovement;
    [SerializeField] private EntityAttackManager _entityAttackManager;

    bool _ignoreInput = false;


    void Awake()
    {
        if (_entityMovement == null)
        {
            _entityMovement = GetComponent<EntityMovement>();
        }
    }

    void OnEnable()
    {
        // Subscribe to the UI Move event
        UIInputManager.OnMoveInput += Move;
        UIInputManager.OnAttackInput += Attack;
        UIInputManager.OnHealInput += Heal;

        CombatManager.Instance.OnTurnChanged += HandleTurns;
    }

    void OnDisable()
    {
        // Unsubscribe from the UI Move event
        UIInputManager.OnMoveInput -= Move;
        UIInputManager.OnAttackInput -= Attack;
        UIInputManager.OnHealInput -= Heal;

        CombatManager.Instance.OnTurnChanged -= HandleTurns;
    }

    void Attack()
    {
        _entityAttackManager.Attack();
    }

    void Heal()
    {
        Debug.Log("Heal");
        // TODO: Implement heal
    }

    void HandleTurns(CombatTurnState turnState)
    {
        switch (turnState)
        {
            case CombatTurnState.Player:
                _ignoreInput = false;
                break;
            case CombatTurnState.Enemy:
                _ignoreInput = true;
                break;
            case CombatTurnState.NonCombat:
                _ignoreInput = false;
                break;
        }
    }

    void Move(MoveDirection direction)
    {
        if (_ignoreInput) return;

        switch (direction)
        {
            case MoveDirection.Up:
                _entityMovement.Move(Vector2Int.up);
                break;
            case MoveDirection.Down:
                _entityMovement.Move(Vector2Int.down);
                break;
            case MoveDirection.Left:
                _entityMovement.Move(Vector2Int.left);
                break;
            case MoveDirection.Right:
                _entityMovement.Move(Vector2Int.right);
                break;
        }
    }
}
