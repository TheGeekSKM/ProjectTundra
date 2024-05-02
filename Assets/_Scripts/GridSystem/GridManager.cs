using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
	public static GridManager Instance;

    [Header("Grid Settings")]
    public Vector2Int gridSize;
    [SerializeField] private Vector2 _cameraOffset;
    [SerializeField] bool _followCamera = false;
    
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
		gridSize = new Vector2Int(MazeGen.Instance.roomWidth, MazeGen.Instance.roomHeight);
		_cameraOffset = new Vector2(gridSize.x / 2,gridSize.y / 2);
	}

	void HandleTurnChange(CombatTurnState state)
	{
		//if (state != CombatTurnState.NonCombat && state != CombatTurnState.CameraMove)
			//FollowCamera();
		//else
		//	ClearAllTiles();
	}

	[ContextMenu("Generate Grid")]
    void GenerateGrid() {

		FollowCamera();

		//check to see if children already exist
		if (transform.childCount > 0) {
            return;
        }

        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < gridSize.x; x++) {
            for (int y = 0; y < gridSize.y; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(transform.position.x+x, transform.position.y+y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                spawnedTile.transform.SetParent(transform);
 
 
                _tiles[new Vector2(x, y)] = spawnedTile;

				//var hit = Physics2D.OverlapCircle(spawnedTile.transform.position, 0.49f);
				//Debug.Log(hit.gameObject.name);
				//if (hit.GetComponent<TilemapCollider2D>())
				//	spawnedTile.walkable = false;
				//else if (hit.GetComponent<ChestManager>())
				//	spawnedTile.gameObject.SetActive(false);
				//else
				//	spawnedTile.walkable = true;
			}
        }
    }


    // move the camera to the center of the grid
    void FollowCamera()
    {
        if (!_followCamera) return;
		float _x = Camera.main.transform.position.x - _cameraOffset.x;
		float _y = Camera.main.transform.position.y - _cameraOffset.y;
		transform.position = new Vector3(_x,_y,transform.position.z);
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
