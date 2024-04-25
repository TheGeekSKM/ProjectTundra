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
		MazeGen maze = MazeGen.Instance;

		roomCoord = RoomManager.Instance.startRoom;
		transform.position = new Vector3((roomCoord.x * maze.roomWidth) + maze.roomCenterOffset.x, 
										 (roomCoord.y * maze.roomHeight)+ maze.roomCenterOffset.y, 
										 transform.position.z);
	}

	public void Move(Vector2 direction)
    {
		MazeGen maze = MazeGen.Instance;
		RoomManager rm = RoomManager.Instance;

		_prevRoom = roomCoord;
		roomCoord += direction;

		RoomController room;
		if (roomCoord == maze.entranceCoords)
			room = rm.entrance;
		else if (roomCoord == maze.exitCoords)
			room = rm.exit;
		else
		{
			room = rm.rooms[(int)roomCoord.x, (int)roomCoord.y];
			room.gameObject.SetActive(true);
		}

		//Set room destination based on direction inputted
		Vector2 dest = room.transform.position;
		Vector3 destination = new Vector3(dest.x, dest.y, transform.position.z);

		//Start movement routine
        StartCoroutine(Movement(destination, rm));
    }
	IEnumerator Movement(Vector3 destination, RoomManager rm)
	{
		MazeGen maze = MazeGen.Instance;

		RoomController room;
		if (roomCoord == maze.entranceCoords)
			room = rm.entrance;
		else if (roomCoord == maze.exitCoords)
			room = rm.exit;
		else
			room = rm.rooms[(int)roomCoord.x, (int)roomCoord.y];

		//Set Camera Move state
		CombatManager.Instance.CameraMoving(true, room);

		//Do the tween and wait for duration to elapse
		transform.DOMove(destination, duration);
		yield return new WaitForSeconds(duration);

		if (_prevRoom.x >= 0 && _prevRoom.x < maze.mazeWidth &&
			_prevRoom.y >= 0 && _prevRoom.y < maze.mazeHeight)
			rm.rooms[(int)_prevRoom.x, (int)_prevRoom.y].gameObject.SetActive(false);

		//Unset Camera Move state
		CombatManager.Instance.CameraMoving(false, room);

		yield break;
	}
}
