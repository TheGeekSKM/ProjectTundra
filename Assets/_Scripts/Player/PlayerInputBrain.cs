using UnityEngine;

[RequireComponent(typeof(EntityMovement))]
public class PlayerInputBrain : MonoBehaviour
{
    [SerializeField] private EntityMovement _entityMovement;

    void OnValidate()
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
    }

    void OnDisable()
    {
        // Unsubscribe from the UI Move event
        UIInputManager.OnMoveInput -= Move;
    }

    void Move(MoveDirection direction)
    {
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
