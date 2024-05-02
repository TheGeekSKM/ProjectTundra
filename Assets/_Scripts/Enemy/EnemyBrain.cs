using System.Collections.Generic;
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
    [SerializeField] EntityAnimationController _entityAnimationController;
    [SerializeField] EntityInventoryManager _entityInventoryManager;

    [Header("Enemy Settings")]
    [SerializeField] bool _useRandomEnemies = true;
    [SerializeField] float _timeBetweenActions = 1f;
    [SerializeField] float _raycastDistance = 1f;
    [SerializeField] LayerMask _obstacleLayerMask;
    Coroutine _currentAction;

    public System.Action<string> OnEnemyTurnEnded;

    bool _isMyTurn = false;
    int _attackRange = 1;

	List<PathNode> path;
	int pathCounter;

	// Grab references to components if they are not set
	void Awake()
    {
        
    }

    void GetReferences()
    {
        if (!_entityStatsContainer) _entityStatsContainer = GetComponent<EntityStatsContainer>();
        if (!_entityAttackManager) _entityAttackManager = GetComponent<EntityAttackManager>();
        if (!_entityMovement)_entityMovement = GetComponent<EntityMovement>();
        if (!_entityHealth)_entityHealth = GetComponent<EntityHealth>();
        if (!_entityStamina)_entityStamina = GetComponent<EntityStamina>();
        if (!_entityLoot)_entityLoot = GetComponent<EntityLoot>();
        if (!_entityAnimationController)_entityAnimationController = GetComponentInChildren<EntityAnimationController>();
        if (!_entityInventoryManager) _entityInventoryManager = GetComponent<EntityInventoryManager>();
    }

    void OnEnable()
    {
        // if the enemy is set to use random enemies, set the enemy stats data to a random enemy
        if (_useRandomEnemies) _entityStatsContainer.SetPlayerStatsData(GameDataManager.Instance.GetRandomEnemy());
        GetReferences();

		StartCoroutine(Enabler());
        //_entityStamina.ResetAP();
        //_attackRange = _entityStatsContainer.PlayerStatsData.ItemContainer.GetWeapon().AttackRange;
        //CombatManager.Instance.AddEnemy(this);
        //_entityHealth.OnHealthChanged += HandleDeathCheck;
        _entityLoot.OnLootDropped += DestroyEntity;
        _entityAnimationController.Initialize();
        _entityHealth.Initialize();
        _entityStamina.Initialize();
        _entityInventoryManager.Initialize();
    }
	IEnumerator Enabler()
	{
		yield return new WaitUntil(() => _entityStamina != null);
		_entityStamina.ResetAP();
		_attackRange = _entityInventoryManager.EntityInventory.GetWeapon().AttackRange;
		CombatManager.Instance.AddEnemy(this);
		_entityHealth.OnHealthChanged += HandleDeathCheck;

		yield break;
	}

	void OnDisable()
	{
        _entityHealth.OnHealthChanged -= HandleDeathCheck;
		_entityLoot.OnLootDropped -= DestroyEntity; // idk if this is necessary
        CombatManager.Instance.RemoveEnemy(this);

	}

    void DestroyEntity()
    {
        Destroy(gameObject);
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
            GameDataManager.Instance.PlayerKillCount++;
        }
    }

    bool HandleTurnCheck()
    {
        if (CombatManager.Instance.CurrentTurnState != CombatTurnState.Enemy) {
            EndTurn();
            return true;
        }
        return false;
    }

    #endregion

    // Start the enemy's turn -> THIS IS MEANT TO BE CALLED BY THE COMBAT MANAGER
    public void StartEnemyTurn()
    {
        Debug.Log("Starting Enemy Turn was called by Combat Manager");
        _isMyTurn = true;
        _entityStamina.ResetAP();

		path = Pathfind();
		pathCounter = 0;

		if (_currentAction != null) StopCoroutine(_currentAction);
        HandleTurnLogic();
    }

    #region TurnLogicHandlers
    void EndTurn()
    {
		if (path != null)
		{
			foreach (PathNode n in path)
				Destroy(n.gameObject);
			path = null;
		}

        _isMyTurn = false;
        OnEnemyTurnEnded?.Invoke(gameObject.name);
    }

    void HandleTurnLogic()
    {
		// Debug.Log("Starting Turn Logic");
        if (HandleTurnCheck()) 
        {
            StopAllCoroutines();
            return;
        }
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
        _entityAttackManager.AttackOrigin.right = Player.Instance.transform.position - _entityAttackManager.AttackOrigin.position;
        
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

	#region Pathfinding
	PathNode[,] GenerateNodeGrid()
	{
		//Debug.Log("Fuction called successfully!");

		MazeGen maze = MazeGen.Instance;
		Camera main = Camera.main;
        // sizeOffset is half the size of the room and is used to center the grid
		Vector2 sizeOffset = new Vector2(maze.roomWidth / 2, maze.roomHeight / 2);

        // pos is the bottom left corner of the room and is used to position the grid
		Vector2 pos = new Vector2(main.transform.position.x - sizeOffset.x, main.transform.position.y - sizeOffset.y);

		PathNode[,] nodeGrid = new PathNode[maze.roomWidth, maze.roomHeight];
		for (int x = 0; x < maze.roomWidth; x++)
		{
			for (int y = 0; y < maze.roomHeight; y++)
			{
				nodeGrid[x, y] = new GameObject("Path Node").AddComponent<PathNode>();
				nodeGrid[x, y].transform.parent = gameObject.transform.parent;

				nodeGrid[x, y].position = new Vector2(pos.x + x + 0.5f, pos.y + y + 0.5f);
				nodeGrid[x, y].gridPos = new Vector2Int(x, y);
				nodeGrid[x, y].transform.position = nodeGrid[x, y].position;

				//Debug.Log("Node created, position is " + nodeGrid[x, y].position);

				var hit = Physics2D.Raycast(nodeGrid[x, y].position, Vector2.zero);
				if (hit.collider != null)
				{
					//Debug.Log("Hit a collider: " + hit.collider.gameObject.name);

					if (hit.collider.CompareTag("Player"))
						nodeGrid[x, y].end = true;
					else if (hit.collider.gameObject == gameObject)
						nodeGrid[x, y].start = true;
					else
						nodeGrid[x, y].walkable = false;
				}
			}
		}

		//Debug.Log("Function completed! Test: " + nodeGrid[0,0]);
		return nodeGrid;
	}
	List<PathNode> Pathfind()
	{
		PathNode[,] nodeGrid = GenerateNodeGrid();

		List<PathNode> open = new List<PathNode>();
		List<PathNode> closed = new List<PathNode>();
		PathNode start = null;
		PathNode end = null;

		foreach (PathNode node in nodeGrid)
		{
			if (node.start)
				start = node;
			if (node.end)
				end = node;
		}
		if (start == null || end == null)
		{
			Debug.LogError("Could not find start or end nodes");
			Debug.Break();
		}

		open.Add(start);

		PathNode current = GetLowestCost(open);
		while (open.Count > 0)
		{
			current = GetLowestCost(open);
			open.Remove(current);
			closed.Add(current);

			if (current == end)
			{
				return RetracePath(start, end, nodeGrid);
			}

			foreach (PathNode neighbor in GetNeighbors(current, nodeGrid))
			{
				if (!neighbor.walkable || closed.Contains(neighbor))
					continue;

				int newMoveCostToNeighbor = current.g_cost + GetDistance(current, neighbor);
				if (newMoveCostToNeighbor < neighbor.g_cost || !open.Contains(neighbor))
				{
					neighbor.g_cost = newMoveCostToNeighbor;
					neighbor.h_cost = GetDistance(neighbor, end);
					neighbor.parent = current;

					if (!open.Contains(neighbor))
						open.Add(neighbor);
				}
			}
		}
		return RetracePath(start, current, nodeGrid);
	}
	PathNode GetLowestCost(List<PathNode> list)
	{
		PathNode selected = list[0];
		foreach (PathNode node in list)
		{
			if (node.f_cost < selected.f_cost || (node.f_cost == selected.f_cost && node.h_cost < selected.h_cost))
				selected = node;
		}
		return selected;
	}
	List<PathNode> GetNeighbors(PathNode node, PathNode[,] grid)
	{
		MazeGen maze = MazeGen.Instance;
		List<PathNode> neighbors = new List<PathNode>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if ((x == 0 && y == 0) ||
					(x == -1 && y == -1) || (x == -1 && y == 1) ||
					(x == 1 && y == -1) || (x == 1 && y == 1))
					continue;

				int checkX = node.gridPos.x + x;
				int checkY = node.gridPos.y + y;

				if (checkX >= 0 && checkX < maze.roomWidth && checkY >= 0 && checkY < maze.roomHeight)
					neighbors.Add(grid[checkX, checkY]);
			}
		}

		return neighbors;
	}
	int GetDistance(PathNode nodeA, PathNode nodeB)
	{
		int distX = Mathf.Abs(nodeA.gridPos.x - nodeB.gridPos.x);
		int distY = Mathf.Abs(nodeA.gridPos.y - nodeB.gridPos.y);

		if (distX > distY)
			return 14 * distY + 10 * (distX - distY);
		return 14 * distX + 10 * (distY - distX);
	}
	List<PathNode> RetracePath(PathNode start, PathNode end, PathNode[,] grid)
	{
		List<PathNode> path = new List<PathNode>();
		PathNode current = end;

		while (current != start)
		{
			path.Add(current);
			current = current.parent;
		}

		path.Reverse();

		foreach (PathNode node in grid)
		{
			if (!path.Contains(node))
				Destroy(node.gameObject);
		}
		return path;
	}
	#endregion

	void HandleMovement()
    {
        // find the direction to next path
        var direction = path[pathCounter].transform.position - transform.position;
		pathCounter++;
		//Debug.Log("Enemy direction is: " + direction);

        // start the movement coroutine
        _currentAction = StartCoroutine(MoveAction(direction));
    }

	IEnumerator MoveAction(Vector3 direction)
    {
		Debug.Log("Enemy Moving!");

        // normalize the direction
        direction.Normalize();

		if (direction.x > 0)
			_entityMovement.MoveRight(true);

		if (direction.x < 0)
			_entityMovement.MoveLeft(true);

		if (direction.y > 0)
			_entityMovement.MoveUp(true);

		if (direction.y < 0)
			_entityMovement.MoveDown(true);

		#region OldMovementLogic
		//// check if the direction's x or y is greater
		//if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
  //      {
  //          //attempt to move in the x direction
  //          if (direction.x > 0)
  //          {
  //              // check if there is a wall in the way
  //              if (Physics2D.Raycast(transform.position, Vector2.right, _raycastDistance, _obstacleLayerMask))
  //              {
  //                  Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");
  //                  ChooseYDirection(direction);
  //              }
  //              else _entityMovement.MoveRight(true); // otherwise move right
                
  //          }
  //          else
  //          {
  //              // check if there is a wall in the way
  //              if (Physics2D.Raycast(transform.position, Vector2.left, _raycastDistance, _obstacleLayerMask))
  //              {
  //                  Debug.Log($"{gameObject.name}: Wall in the way, moving in the y direction");
  //                  ChooseYDirection(direction);
  //              }
  //              else _entityMovement.MoveLeft(true); // otherwise move left
  //          }
  //      }
  //      else
  //      {
  //          // move in the y direction
  //          if (direction.y > 0)
  //          {
  //              // check if there is a wall in the way
  //              if (Physics2D.Raycast(transform.position, Vector2.up, _raycastDistance, _obstacleLayerMask))
  //              {
  //                  Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
  //                  ChooseXDirection(direction);
  //              }
  //              else _entityMovement.MoveUp(true);
  //          }
  //          else
  //          {
  //              // check if there is a wall in the way
  //              if (Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, _obstacleLayerMask))
  //              {
  //                  Debug.Log($"{gameObject.name}: Wall in the way, moving in the x direction");
  //                  ChooseXDirection(direction);
  //              }
  //              else _entityMovement.MoveDown(true);
  //          }
  //      }

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

	#region OldMovementLogic
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
	#endregion

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
