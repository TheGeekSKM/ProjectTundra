using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	public static RoomManager Instance { get; private set; }

	public GameObject[,] rooms;

	public Vector2 startRoom;

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
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
		rooms = new GameObject[maze.mazeWidth, maze.mazeHeight];

		for (int x = 0; x < maze.mazeWidth; x++)
		{
			for (int y = 0; y < maze.mazeHeight; y++)
			{
				Debug.Log(maze.transform.GetChild(x + (y+x)).gameObject.name);
				rooms[x, y] = maze.transform.GetChild(x+(y+x)).gameObject;

				if (!(x == (int)startRoom.x && y == (int)startRoom.y))
					rooms[x, y].SetActive(false);
			}
		}
	}
}
