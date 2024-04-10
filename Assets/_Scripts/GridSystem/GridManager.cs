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
    }

    void OnDisable()
    {
        TouchManager.Instance.OnTap -= TouchPerformed;
    }
 
    void Start() {
        UpdateCamera();
    }
 
    [ContextMenu("Generate Grid")]
    void GenerateGrid() {
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
}
