using UnityEngine;

public enum MoveDirection
{
    Up,
    Down,
    Left,
    Right
}

public class UIInputManager : MonoBehaviour
{
    #region Singleton
    public static UIInputManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // this is static so we can subscribe to it from anywhere
    public static event System.Action<MoveDirection> OnMoveInput;
    public static event System.Action OnAttackInput;
    public static event System.Action OnHealInput;

    public void MoveUp()
    {
        OnMoveInput?.Invoke(MoveDirection.Up);
    }

    public void MoveDown()
    {
		OnMoveInput?.Invoke(MoveDirection.Down);
    }

    public void MoveLeft()
    {
		OnMoveInput?.Invoke(MoveDirection.Left);
    }

    public void MoveRight()
    {
		OnMoveInput?.Invoke(MoveDirection.Right);
    }

    public void Attack()
    {
        OnAttackInput?.Invoke();
    }

    public void Heal()
    {
        OnHealInput?.Invoke();
    }

    public void Move()
    {
        Debug.Log("Move");
    }
}
