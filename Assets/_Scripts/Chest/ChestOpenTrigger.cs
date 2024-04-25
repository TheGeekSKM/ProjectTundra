using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChestOpenTrigger : MonoBehaviour
{
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] ChestManager _chestManager;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _chestManager = GetComponentInParent<ChestManager>();
        _collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player) _chestManager.SetUseable(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player) _chestManager.SetUseable(false);    
    }
}
