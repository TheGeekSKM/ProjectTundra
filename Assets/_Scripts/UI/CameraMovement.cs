using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public float duration;

	//public event System.Action OnCameraMovementFinish;

	public Vector2 roomCoord = new Vector2(0, 0);

    public void Move(Vector2 direction)
    {
		roomCoord += direction;

		//Set room destination based on direction inputted
		Vector2 dest = RoomManager.Instance.rooms[(int)roomCoord.x, (int)roomCoord.y].transform.position;
		Vector3 destination = new Vector3(dest.x, dest.y, transform.position.z);

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
