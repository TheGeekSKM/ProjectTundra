using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _tilePrefab;
 
    [SerializeField] private Transform _cam;
 
    private Dictionary<Vector2, Tile> _tiles;
    [SerializeField] private Tile _previousTile;

    void OnEnable()
    {
        TouchManager.Instance.OnTap += TouchPerformed;
    }

    void OnDisable()
    {
        TouchManager.Instance.OnTap -= TouchPerformed;
    }
 
    void Start() {

    }
 
    [ContextMenu("Generate Grid")]
    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                spawnedTile.transform.SetParent(transform);
 
 
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
 
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height / 2 - 0.5f,-10);
    }

    void TouchPerformed(Vector2 touchPosition) {
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10));

        var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        
        if (hit.collider != null) {
            var tile = hit.collider.GetComponent<Tile>();
            if (tile != null) {
                HighlightTile(tile.transform.position);
            }
        }
    }

    void HighlightTile(Vector2 pos) {
        var tile = GetTileAtPosition(pos);
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
