using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	public static RoomManager Instance { get; private set; }

	public RoomController[,] rooms;

	public Vector2 startRoom;

	[HideInInspector]
	public RoomController entrance, exit;

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);

		Camera.main.GetComponent<CameraMovement>().SetRoomCoord();
	}

	private void OnEnable()
	{
		MazeGen.Instance.OnGenerationComplete += SetRooms;
	}

	private void OnDisable()
	{
		MazeGen.Instance.OnGenerationComplete -= SetRooms;
	}

	void SetRooms()
	{
		MazeGen maze = MazeGen.Instance;
		rooms = new RoomController[maze.mazeWidth, maze.mazeHeight];

		for (int x = 0; x < maze.mazeWidth; x++)
		{
			for (int y = 0; y < maze.mazeHeight; y++)
			{
				Debug.Log(maze.transform.GetChild((x*maze.mazeHeight)+y).gameObject.name);
				rooms[x, y] = maze.transform.GetChild((x*maze.mazeHeight)+y).GetComponent<RoomController>();

				if (!(x == (int)startRoom.x && y == (int)startRoom.y))
					rooms[x, y].gameObject.SetActive(false);
			}
		}

		entrance = maze.transform.GetChild(maze.transform.childCount - 2).GetComponent<RoomController>();
		exit = maze.transform.GetChild(maze.transform.childCount - 1).GetComponent<RoomController>();
	}

	[ContextMenu("Reset Rooms")]
	public void ResetRooms()
	{
		foreach (RoomController room in rooms)
		{
			room.ResetRoom();
		}

		Camera.main.GetComponent<CameraMovement>().SetRoomCoord();
	}
}
