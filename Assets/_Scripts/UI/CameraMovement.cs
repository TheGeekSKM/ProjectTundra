using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public float duration;

    //public event System.Action OnCameraMovementFinish;

    public void Move(Vector2 direction)
    {
		//Set room destination based on direction inputted
		Vector3 destination = transform.position;
		destination.x += direction.x * MazeGen.Instance.roomWidth;
		destination.y += direction.y * MazeGen.Instance.roomHeight;

		//Start movement routine
        StartCoroutine(Movement(destination));
    }
	IEnumerator Movement(Vector3 destination)
	{
		//Set Camera Move state
		CombatManager.Instance.CameraMoving(true);

		//Do the tween and wait for duration to elapse
		transform.DOMove(destination, duration);
		yield return new WaitForSeconds(duration);

		//Unset Camera Move state
		CombatManager.Instance.CameraMoving(false);

		yield break;
	}
}
