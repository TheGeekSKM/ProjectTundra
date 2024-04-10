using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _useCustomSprite = false;
 
    public virtual void Init(bool isOffset) {
        if (_useCustomSprite) return;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    public virtual void Highlight() {
        _highlight.SetActive(true);
    }

    public virtual void Deselect() {
        _highlight.SetActive(false);
    }
 
}
