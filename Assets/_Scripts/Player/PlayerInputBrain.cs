using UnityEngine;

[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(AudioSource))]
public class PlayerInputBrain : MonoBehaviour
{
    [SerializeField] private EntityMovement _entityMovement;
    [SerializeField] private EntityAttackManager _entityAttackManager;

    bool _ignoreInput = false;

	//Audio
	[Header("Audio")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip _audioMove;
	[SerializeField] private AudioClip _audioHeal;
	[SerializeField] private AudioClip _audioAttack;

	void OnValidate()
	{
		if (_entityMovement == null)
			_entityMovement = GetComponent<EntityMovement>();
	}

	void Awake()
	{
		if (_entityMovement == null)
			_entityMovement = GetComponent<EntityMovement>();
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
		//Audio
		audioSource.clip = _audioAttack;
		audioSource.Play();

        _entityAttackManager.Attack();
    }

    void Heal()
    {
		//Audio
		audioSource.clip = _audioHeal;
		audioSource.Play();

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

		//Audio
		audioSource.clip = _audioMove;
		audioSource.Play();

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
