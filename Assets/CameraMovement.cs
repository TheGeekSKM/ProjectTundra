using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float duration;

    public event System.Action OnCameraMovementFinish;

    public void Move(Vector3 destination)
    {
        StartCoroutine(Movement(destination));
    }
    IEnumerator Movement(Vector3 destination)
    {
        transform.DOMove(destination, duration, true);
        yield return new WaitForSeconds(duration);
        OnCameraMovementFinish.Invoke();
        yield break;
    }

}
