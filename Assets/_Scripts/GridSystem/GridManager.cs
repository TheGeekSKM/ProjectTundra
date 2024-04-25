using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
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
 
    void Start() {
        UpdateCamera();
    }

	void HandleTurnChange(CombatTurnState state)
	{
		//if (state != CombatTurnState.NonCombat)
		//{
		//	GenerateGrid();
		//}

		//else
		//{
		//	ClearAllTiles();
		//}
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
                spawnedTile.Init(isOffset);

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
			//Debug.Log("Hit a collider!");

            var tile = hit.collider.GetComponent<Tile>();
            if (tile != null) {
				//Debug.Log("Hit a tile!");
				HighlightTile(tile);
            }
        }
		else if (_previousTile != null)
		{
			_previousTile.Deselect();
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


    void HighlightTile(Tile tile) {
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
}
