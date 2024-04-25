using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public float duration;

	//public event System.Action OnCameraMovementFinish;

	public Vector2 roomCoord;

	private Vector2 _prevRoom;

	public void SetRoomCoord()
	{
		roomCoord = RoomManager.Instance.startRoom;
	}

	public void Move(Vector2 direction)
    {
		RoomManager rm = RoomManager.Instance;

		_prevRoom = roomCoord;
		roomCoord += direction;

		rm.rooms[(int)roomCoord.x, (int)roomCoord.y].gameObject.SetActive(true);

		//Set room destination based on direction inputted
		Vector2 dest = rm.rooms[(int)roomCoord.x, (int)roomCoord.y].transform.position;
		Vector3 destination = new Vector3(dest.x, dest.y, transform.position.z);

		//Start movement routine
        StartCoroutine(Movement(destination, rm));
    }
	IEnumerator Movement(Vector3 destination, RoomManager rm)
	{
		//Set Camera Move state
		CombatManager.Instance.CameraMoving(true, rm.rooms[(int)roomCoord.x, (int)roomCoord.y]);

		//Do the tween and wait for duration to elapse
		transform.DOMove(destination, duration);
		yield return new WaitForSeconds(duration);

		rm.rooms[(int)_prevRoom.x, (int)_prevRoom.y].gameObject.SetActive(false);

		//Unset Camera Move state
		CombatManager.Instance.CameraMoving(false, rm.rooms[(int)roomCoord.x, (int)roomCoord.y]);

		yield break;
	}
}
