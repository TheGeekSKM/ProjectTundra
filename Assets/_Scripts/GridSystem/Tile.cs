using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _useCustomSprite = false;

    [SerializeField] Tile _connection;
    public Tile Connection => _connection;
    [SerializeField] float _gCost;
    public float GCost => _gCost;
    [SerializeField] float _hCost;
    public float HCost => _hCost;
    public float FCost => _gCost + _hCost;
    [SerializeField] List<Tile> _neighbors;
    public List<Tile> Neighbors => _neighbors;

    [SerializeField] bool _isWalkable = true;
    public bool IsWalkable => _isWalkable;
 
    public virtual void Init(bool isOffset, GridManager gridManager) {
        if (_useCustomSprite) return;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    
        GrabTileNeighbors(gridManager);

        CheckWalkable();
    }

    void GrabTileNeighbors(GridManager gridManager)
    {
        // get the tile above this one
        if (gridManager.GetTileAtPosition(transform.position + Vector3.up, out Tile tile)) 
            _neighbors.Add(tile);
        // get the tile below this one
        if (gridManager.GetTileAtPosition(transform.position + Vector3.down, out tile)) 
            _neighbors.Add(tile);
        // get the tile to the right of this one
        if (gridManager.GetTileAtPosition(transform.position + Vector3.right, out tile)) 
            _neighbors.Add(tile);
        // get the tile to the left of this one
        if (gridManager.GetTileAtPosition(transform.position + Vector3.left, out tile)) 
            _neighbors.Add(tile);
    }

    void CheckWalkable()
    {
        // do a physics circle cast to see if there is a tilemap collider at this position
        if (Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Walls")))
        {
            _isWalkable = false;
        }
    }

    public float GetDistance(Tile other) 
    {
        // create a vector2int to store the distance between the two tiles
        Vector2Int dist = new Vector2Int(
            Mathf.Abs((int)transform.position.x - (int)other.transform.position.x), 
            Mathf.Abs((int)transform.position.y - (int)other.transform.position.y));

        // get the lowest and highest distance
        int lowest = Mathf.Min(dist.x, dist.y);
        int highest = Mathf.Max(dist.x, dist.y);

        // calculate the diagonal moves required
        int horizontalMovesRequired = highest - lowest;

        // return the diagonal moves required * 14 + the horizontal moves required * 10
        return lowest * 14 + horizontalMovesRequired * 10;
    }

    public virtual void Highlight() {
        _highlight.SetActive(true);
    }

    public virtual void Deselect() {
        _highlight.SetActive(false);
    }

    public void SetConnection(Tile tile) {
        _connection = tile;
    }

    public void SetGCost(float cost) {
        _gCost = cost;
    }

    public void SetHCost(float cost) {
        _hCost = cost;
    }
 
}
