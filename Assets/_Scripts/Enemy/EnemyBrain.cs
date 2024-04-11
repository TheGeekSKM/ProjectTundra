using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyBrain : MonoBehaviour
{
    [SerializeField] EntityStatsContainer _entityStatsContainer;
    [SerializeField] EntityAttackManager _entityAttackManager;
    [SerializeField] EntityMovement _entityMovement;

    [SerializeField] float _timeBetweenActions = 1f;
    Coroutine _currentAction;

    public System.Action OnEnemyTurnEnded;

    bool _isMyTurn = false;
    int _attackRange = 1;

	//Audio
	[Header("Audio")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip _audioEnemyAttack;
	[SerializeField] private AudioClip _audioEnemyMove;

    // Grab references to components if they are not assigned
    void OnValidate()
    {
        if (_entityStatsContainer == null) _entityStatsContainer = GetComponent<EntityStatsContainer>();
        if (_entityAttackManager == null) _entityAttackManager = GetComponent<EntityAttackManager>();
        if (_entityMovement == null) _entityMovement = GetComponent<EntityMovement>();
    }

    // Subscribe to events
    void OnEnable()
    {
        // _entityStatsContainer.PlayerStatsData.OnCurrentActionPointsChanged += HandleAPCheck;
        // _entityStatsContainer.PlayerStatsData.OnHealthChanged += HandleDeathCheck;
    }

    void OnDisable()
    {
        // _entityStatsContainer.PlayerStatsData.OnCurrentActionPointsChanged -= HandleAPCheck;
        // _entityStatsContainer.PlayerStatsData.OnHealthChanged -= HandleDeathCheck;
    }

    void Start()
    {
        _entityStatsContainer.PlayerStatsData.ResetActionPoints();

        _attackRange = _entityStatsContainer.ItemContainer.GetWeapon().AttackRange;

        CombatManager.Instance.AddEnemy(this);
    }

    // Check if the enemy has enough action points to take a turn
    void HandleAPCheck()
    {
        // Debug.Log($"Enemy Current AP: {_entityStatsContainer.PlayerStatsData.CurrentActionPoints}");
        if (_entityStatsContainer.PlayerStatsData.CurrentActionPoints <= 0)
        {
            _isMyTurn = false;
            OnEnemyTurnEnded?.Invoke();
        }
        else
        {
            _isMyTurn = true;
        }
    }

    // Check if the enemy has died
    void HandleDeathCheck()
    {
        if (_entityStatsContainer.PlayerStatsData.Health <= 0)
        {
            _isMyTurn = false;
            OnEnemyTurnEnded?.Invoke();
            CombatManager.Instance.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }

    // Start the enemy's turn -> THIS IS MEANT TO BE CALLED BY THE COMBAT MANAGER
    public void StartEnemyTurn()
    {
        _isMyTurn = true;
        _entityStatsContainer.PlayerStatsData.ResetActionPoints();
        if (_currentAction != null) StopCoroutine(_currentAction);
        HandleTurnLogic();
    }


    void HandleTurnLogic()
    {
		// Debug.Log("Starting Turn Logic");

        // check if the enemy has enough action points to take a turn and if they are still alive
        HandleAPCheck();
        HandleDeathCheck();

        // if it's not the enemy's turn, return
        if (!_isMyTurn) return;

        // if there is a current action, stop it
        if (_currentAction != null) StopCoroutine(_currentAction);

        // get the distance between the enemy and the player
        var dist = Vector3.Distance(transform.position, Player.Instance.transform.position);
		Debug.Log("Distance between enemy and player is: " + dist);

        // the attack point should always look at the player
        _entityAttackManager.AttackPoint.LookAt(Player.Instance.transform);
        
        // if the player is within attack range, attack, otherwise move
        if (dist <= _attackRange)
        {
            HandleAttack();
        }
        else
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        // find the direction to the player
        var direction = Player.Instance.transform.position - transform.position;

        // start the movement coroutine
        _currentAction = StartCoroutine(MoveAction(direction));
        
    }

    IEnumerator MoveAction(Vector3 direction)
    {
		Debug.Log("Enemy Moving!");

		//Audio
		audioSource.clip = _audioEnemyMove;
		audioSource.Play();

        // check if the direction's x or y is greater
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // move in the x direction
            if (direction.x > 0)
            {
				// Debug.Log("Enemy Moving Right!");
                _entityMovement.MoveRight();
            }
            else
            {
				// Debug.Log("Enemy Moving Left!");
				_entityMovement.MoveLeft();
            }
        }
        else
        {
            // move in the y direction
            if (direction.y > 0)
            {
				// Debug.Log("Enemy Moving Up!");
				_entityMovement.MoveUp();
            }
            else
            {
				// Debug.Log("Enemy Moving Down!");
				_entityMovement.MoveDown();
            }
        }

        // once movement is done, wait for the time between actions
        yield return new WaitForSeconds(_timeBetweenActions);
        HandleTurnLogic();
    }

    void HandleAttack()
    {
		//Audio
		audioSource.clip = _audioEnemyAttack;
		audioSource.Play();

		_entityAttackManager.Attack();
        _currentAction = StartCoroutine(WaitForNextAction());
    }

    IEnumerator WaitForNextAction()
    {
        yield return new WaitForSeconds(_timeBetweenActions);
        HandleTurnLogic();
    }

    // void Update()
    // {
    //     if (!_isMyTurn) return;

    //     if (Vector3.Distance(transform.position, Player.Instance.transform.position) <= _attackRange)
    //     {
    //         HandleAttack();
    //     }
    //     else
    //     {
    //         HandleMovement();
    //     }
    // }


}
