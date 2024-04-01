using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
 
    public void Init(bool isOffset) {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnEnable()
    {
        TouchManager.Instance.OnTouchPerformed += OnTouchPerformed;
    }

    void OnDisable()
    {
        TouchManager.Instance.OnTouchPerformed -= OnTouchPerformed;
    }

    private void OnTouchPerformed(Vector2 touchPosition)
    {
        // touchPosition = new Vector2(touchPosition.x, Camera.main.pixelHeight - touchPosition.y);
        touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        var hit = Physics2D.Raycast(touchPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            _highlight.SetActive(true);
            Debug.Log("Tile Clicked");
        }
        else
        {
            _highlight.SetActive(false);
            // Debug.Log("Tile Not Clicked");
        }
    }
 
}
