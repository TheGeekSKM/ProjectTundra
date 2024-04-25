using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected Color _baseColor, _offsetColor;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected GameObject _highlight;
    [SerializeField] protected bool _useCustomSprite = false;

	public bool walkable = false;
 
    public virtual void Init(bool isOffset, bool useCustomSprite = false) {
        if (_useCustomSprite) return;
        _renderer.color = isOffset ? _offsetColor : _baseColor;

        _useCustomSprite = useCustomSprite;
    }

    public virtual void Highlight() {
        _highlight.SetActive(true);
    }

    public virtual void Deselect() {
        _highlight.SetActive(false);
    }
 
}
