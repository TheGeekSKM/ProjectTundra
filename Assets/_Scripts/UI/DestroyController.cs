using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
    [SerializeField] float _timeBeforeDestruction;
    void Start() => Destroy(gameObject, _timeBeforeDestruction);
}
