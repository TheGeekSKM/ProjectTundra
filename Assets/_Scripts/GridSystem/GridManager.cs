using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [Header("Grid Settings")]
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _cameraOffset;
    [SerializeField] bool _moveCamera = false;
    
    [Header("References")]
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] private Tile _previousTile;
    
    private Dictionary<Vector2, Tile> _tiles;

    void OnEnable()
    {
        TouchManager.Instance.OnTap += TouchPerformed;
        CombatManager.Instance.OnTurnChanged += HandleTurnChange;
    }

    void OnDisable()
    {
        TouchManager.Instance.OnTap -= TouchPerformed;
        CombatManager.Instance.OnTurnChanged -= HandleTurnChange;
    }
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() {
        UpdateCamera();
    }

    void HandleTurnChange(CombatTurnState state)
    {
        if (state != CombatTurnState.NonCombat)
        {
            GenerateGrid();
        }
        else
        {
            ClearAllTiles();
        }
    }
 
    [ContextMenu("Generate Grid")]
    void GenerateGrid() {

        //check to see if children already exist
        if (transform.childCount > 0) {
            return;
        }

        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _gridSize.x; x++) {
            for (int y = 0; y < _gridSize.y; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, this);

                spawnedTile.transform.SetParent(transform);
 
 
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        

        UpdateCamera();
    }


    // move the camera to the center of the grid
    void UpdateCamera()
    {
        if (!_moveCamera) return;
        float _x = (float)(_gridSize.x + _cameraOffset.x) / 2 - 0.5f;
        float _y = (float)(_gridSize.y + _cameraOffset.y) / 2 - 0.5f;
        _cam.transform.position = new Vector3(_x, _y, -10);
    }

    // run this function when a touch is performed
    void TouchPerformed(Vector2 touchPosition) {
        if (ChestViewManager.Instance.IsChestOpen) return;
        
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10));

        var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        
        if (hit.collider != null) {
            var tile = hit.collider.GetComponent<Tile>();
            if (tile != null) {
				//HighlightTile(tile.transform.position);
				HighlightTile(tile);
            }
        }
    }

    [ContextMenu("Clear All Tiles")]
    public void ClearAllTiles() {
        
        // if the tiles dictionary is null, then delete all children of the grid
        if (_tiles == null) 
        {
            //deletes all children of the grid
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
            return;
        }

        // if the tiles dictionary is not null, then delete all tiles in the dictionary
        foreach (var tile in _tiles.Values) {
            Destroy(tile.gameObject);
        }
        _tiles.Clear();
    }


	//void HighlightTile(Vector2 pos) {
    void HighlightTile(Tile tile) {
        //var tile = GetTileAtPosition(pos);
        if (tile != null) {
            tile.Highlight();
            if (_previousTile != null && _previousTile != tile) {
                _previousTile.Deselect();
                _previousTile = tile;
            }
            else
            {
                _previousTile = tile;
            }
        }
    }
 
    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

     public bool GetTileAtPosition(Vector2 pos, out Tile tile) {
        if (_tiles.TryGetValue(pos, out tile)) return true;
        return false;
    }


    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        // make a list of both the open and closed tiles
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        // add the start tile to the open list
        openList.Add(startTile);

        // while there are still tiles in the open list
        while (openList.Count > 0)
        {
            Tile current = openList[0];

            // find the tile with the lowest F cost
            foreach (Tile tile in openList)
            {
                if (tile.FCost < current.FCost || tile.FCost == current.FCost && tile.HCost < current.HCost)
                {
                    current = tile;
                }
            }

            // remove the current tile from the open list and add it to the closed list
            openList.Remove(current);
            closedList.Add(current);

            // if the current tile is the target tile, then return the path
            if (current == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            // loop through each neighbor of the current tile
            foreach (Tile neighbor in current.Neighbors.Where(t => t.IsWalkable && !closedList.Contains(t)))
            {
                bool isNeighborInOpenList = openList.Contains(neighbor);

                // calculate the new G cost to the neighbor
                float newMovementCostToNeighbor = current.GCost + current.GetDistance(neighbor);

                if (!isNeighborInOpenList || newMovementCostToNeighbor < neighbor.GCost)
                {
                    neighbor.SetGCost(newMovementCostToNeighbor);
                    neighbor.SetConnection(current);

                    if (!isNeighborInOpenList)
                    {
                        neighbor.SetHCost(neighbor.GetDistance(targetTile));
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    ///  Retrace the path from the start tile to the end tile
    /// </summary>
    List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Connection;
        }

        path.Reverse();
        return path;
    }

}
