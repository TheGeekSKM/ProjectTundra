using System.Collections;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [Header("Enemy Components")]
    [SerializeField] EntityStatsContainer _entityStatsContainer;
    [SerializeField] EntityAttackManager _entityAttackManager;
    [SerializeField] EntityMovement _entityMovement;
    [SerializeField] EntityHealth _entityHealth;
    [SerializeField] EntityStamina _entityStamina;
    [SerializeField] EntityLoot _entityLoot;

    [Header("Enemy Settings")]
    [SerializeField] float _timeBetweenActions = 1f;
    [SerializeField] float _raycastDistance = 1f;
    [SerializeField] LayerMask _obstacleLayerMask;
    Coroutine _currentAction;

    public System.Action<string> OnEnemyTurnEnded;

    bool _isMyTurn = false;
    int _attackRange = 1;

    // Grab references to components if they are not set
    void Awake()
    {
        if (_entityStatsContainer == null) _entityStatsContainer = GetComponent<EntityStatsContainer>();
        if (_entityAttackManager == null) _entityAttackManager = GetComponent<EntityAttackManager>();
        if (_entityMovement == null) _entityMovement = GetComponent<EntityMovement>();
        if (_entityHealth == null) _entityHealth = GetComponent<EntityHealth>();
        if (_entityStamina == null) _entityStamina = GetComponent<EntityStamina>();
        if (_entityLoot == null) _entityLoot = GetComponent<EntityLoot>();
    }

    void OnEnable()
    {
		StartCoroutine(Enabler());
        //_entityStamina.ResetAP();
        //_attackRange = _entityStatsContainer.PlayerStatsData.ItemContainer.GetWeapon().AttackRange;
        //CombatManager.Instance.AddEnemy(this);
        //_entityHealth.OnHealthChanged += HandleDeathCheck;
    }
	IEnumerator Enabler()
	{
		yield return new WaitUntil(() => _entityStamina != null);
		_entityStamina.ResetAP();
		_attackRange = _entityStatsContainer.PlayerStatsData.ItemContainer.GetWeapon().AttackRange;
		CombatManager.Instance.AddEnemy(this);
		_entityHealth.OnHealthChanged += HandleDeathCheck;

        // wait for the loot to be dropped before destroying the enemy
        _entityLoot.OnLootDropped += () => Destroy(gameObject);
		yield break;
	}

	void OnDisable()
	{
        _entityHealth.OnHealthChanged -= HandleDeathCheck;
		_entityLoot.OnLootDropped -= () => Destroy(gameObject); // idk if this is necessary
        CombatManager.Instance.RemoveEnemy(this);

	}

	#region CheckingMethods
	// Check if the enemy has enough action points to take a turn
	void HandleAPCheck()
    {
        Debug.Log($"Enemy Current AP: {_entityStamina.CurrentActionPoints}");
        // Debug.Log($"Enemy Current AP: {_entityStatsContainer.PlayerStatsData.CurrentActionPoints}");

        // if the enemy has no action points, end their turn otherwise, it's their turn
        if (_entityStamina.CurrentActionPoints <= 0) EndTurn();
        else _isMyTurn = true;
    }

    // Check if the enemy has died
    void HandleDeathCheck(int damage)
    {
        if (_entityHealth.CurrentHealth <= 0)
        {
            EndTurn();
            CombatManager.Instance.RemoveEnemy(this);
        }
    }

    #endregion

    // Start the enemy's turn -> THIS IS MEANT TO BE CALLED BY THE COMBAT MANAGER
    public void StartEnemyTurn()
    {
        Debug.Log("Starting Enemy Turn was called by Combat Manager");
        _isMyTurn = true;
        _entityStamina.ResetAP();
        if (_currentAction != null) StopCoroutine(_currentAction);
        HandleTurnLogic();
    }

    #region TurnLogicHandlers
    void EndTurn()
    {
        _isMyTurn = false;
        OnEnemyTurnEnded?.Invoke(gameObject.name);
    }

    void HandleTurnLogic()
    {
		// Debug.Log("Starting Turn Logic");

        // check if the enemy has enough action points to take a turn and if they are still alive
        HandleAPCheck();
        HandleDeathCheck(0);

        // if it's not the enemy's turn, return
        if (!_isMyTurn) return;

        // if there is a current action, stop it
        if (_currentAction != null) StopCoroutine(_currentAction);

        // get the distance between the enemy and the player
        var dist = Vector3.Distance(transform.position, Player.Instance.transform.position);
		// Debug.Log("Distance between enemy and player is: " + dist);

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

        // normalize the direction
        direction.Normalize();
        
        // check if the direction's x or y is greater
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //attempt to move in the x direction
            if (direction.x > 0)
            {
                // check if there is a wall in the way
                if (Physics2D.Raycast(transform.position, Vector2.right, _raycastDistance, _obstacleLayerMask))
                {
                    Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");
                    ChooseYDirection(direction);
                }
                else _entityMovement.MoveRight(true); // otherwise move right
                
            }
            else
            {
                // check if there is a wall in the way
                if (Physics2D.Raycast(transform.position, Vector2.left, _raycastDistance, _obstacleLayerMask))
                {
                    Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");
                    ChooseYDirection(direction);
                }
                else _entityMovement.MoveLeft(true); // otherwise move left
            }
        }
        else
        {
            // move in the y direction
            if (direction.y > 0)
            {
                // check if there is a wall in the way
                if (Physics2D.Raycast(transform.position, Vector2.up, _raycastDistance, _obstacleLayerMask))
                {
                    Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
                    ChooseXDirection(direction);
                }
                else _entityMovement.MoveUp(true);
            }
            else
            {
                // check if there is a wall in the way
                if (Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, _obstacleLayerMask))
                {
                    Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
                    ChooseXDirection(direction);
                }
                else _entityMovement.MoveDown(true);
            }
        }

        #region OldMovementLogic
        // // check if the direction's x or y is greater
        // if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        // {
        //     // move in the x direction
        //     if (direction.x > 0)
        //     {
        //         // if a wall is in the way, move in the y direction -> Right
        //         if (Physics2D.Raycast(transform.position, Vector2.right, _raycastDistance, _obstacleLayerMask))
        //         {
        //             Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");

                    
        //         }
        //         else _entityMovement.MoveRight(true); // otherwise move right
        //     }
        //     else
        //     {
        //         // if a wall is in the way, move in the y direction -> Left
		// 		if (Physics2D.Raycast(transform.position, Vector2.left, _raycastDistance, _obstacleLayerMask))
        //         {
        //             Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");
        //             // randomly pick the y direction
        //             if (Random.Range(0, 2) == 0) _entityMovement.MoveUp(true);
        //             else _entityMovement.MoveDown(true);
        //         }
        //         else _entityMovement.MoveLeft(true); // otherwise move left
        //     }
        // }
        // else
        // {
        //     // move in the y direction
        //     if (direction.y > 0)
        //     {
        //         // if a wall is in the way, move in the x direction -> Up
		// 		if (Physics2D.Raycast(transform.position, Vector2.up, _raycastDistance, _obstacleLayerMask))
        //         {
        //             Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
        //             // randomly pick the x direction
        //             if (Random.Range(0, 2) == 0) _entityMovement.MoveRight(true);
        //             else _entityMovement.MoveLeft(true);
        //         }
        //         else _entityMovement.MoveUp(true);
        //     }
        //     else
        //     {
        //         // if a wall is in the way, move in the x direction -> Down
		// 		if (Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, _obstacleLayerMask))
        //         {
        //             Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
        //             // randomly pick the x direction
        //             if (Random.Range(0, 2) == 0) _entityMovement.MoveRight(true);
        //             else _entityMovement.MoveLeft(true);
        //         }
        //         else _entityMovement.MoveDown(true);
        //     }
        // }
        #endregion
        // once movement is done, wait for the time between actions
        yield return new WaitForSeconds(_timeBetweenActions);
        HandleTurnLogic();
    }


    /// <summary>
    ///  Choose the y direction to move in
    /// </summary>
    void ChooseYDirection(Vector2 direction)
    {
        if (direction.y > 0)
        {
            // if a wall is in the way, move in the x direction -> Up
            if (Physics2D.Raycast(transform.position, Vector2.up, _raycastDistance, _obstacleLayerMask))
            {
                Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
                // randomly pick the x direction
                if (Random.Range(0, 2) == 0) _entityMovement.MoveRight(true);
                else _entityMovement.MoveLeft(true);
            }
            else _entityMovement.MoveUp(true);
        }
        else
        {
            // if a wall is in the way, move in the x direction -> Down
            if (Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, _obstacleLayerMask))
            {
                Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
                // randomly pick the x direction
                if (Random.Range(0, 2) == 0) _entityMovement.MoveRight(true);
                else _entityMovement.MoveLeft(true);
            }
            else _entityMovement.MoveDown(true);
        }
    }

    /// <summary>
    ///  Choose the x direction to move in
    /// </summary> 
    void ChooseXDirection(Vector2 direction)
    {
        if (direction.x > 0)
        {
            // if a wall is in the way, move in the y direction -> Right
            if (Physics2D.Raycast(transform.position, Vector2.right, _raycastDistance, _obstacleLayerMask))
            {
                Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");
                ChooseYDirection(direction);
            }
            else _entityMovement.MoveRight(true); // otherwise move right
        }
        else
        {
            // if a wall is in the way, move in the y direction -> Left
            if (Physics2D.Raycast(transform.position, Vector2.left, _raycastDistance, _obstacleLayerMask))
            {
                Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");
                ChooseYDirection(direction);
            }
            else _entityMovement.MoveLeft(true); // otherwise move left
        }
    }

    void HandleAttack()
    {
        _entityAttackManager.Attack();
        _currentAction = StartCoroutine(WaitForNextAction());
    }

    IEnumerator WaitForNextAction()
    {
        yield return new WaitForSeconds(_timeBetweenActions);
        HandleTurnLogic();
    }
    #endregion

    #region DebugMethods

    [ContextMenu("Kill Enemy")]
    void KillEnemy()
    {
        _entityHealth.Die(); 
        HandleDeathCheck(0);
    }

    #endregion

}
