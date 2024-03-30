using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
	//Room Prefabs
	[Header("Room Prefabs")]

	[Header("Caps")]
	[SerializeField]
	private GameObject[] capNorthExit = new GameObject[1];
	[SerializeField]
	private GameObject[] capEastExit = new GameObject[1];
	[SerializeField]
	private GameObject[] capSouthExit = new GameObject[1];
	[SerializeField]
	private GameObject[] capWestExit = new GameObject[1];

	[Header("Halls")]
	[SerializeField]
	private GameObject[] hallHorizontal = new GameObject[1];
	[SerializeField]
	private GameObject[] hallVertical = new GameObject[1];

	[Header("Hooks")]
	[SerializeField]
	private GameObject[] hookNorthEast = new GameObject[1];
	[SerializeField]
	private GameObject[] hookSouthEast = new GameObject[1];
	[SerializeField]
	private GameObject[] hookSouthWest = new GameObject[1];
	[SerializeField]
	private GameObject[] hookNorthWest = new GameObject[1];

	[Header("T-Shapes")]
	[SerializeField]
	private GameObject[] tNorth = new GameObject[1];
	[SerializeField]
	private GameObject[] tEast = new GameObject[1];
	[SerializeField]
	private GameObject[] tSouth = new GameObject[1];
	[SerializeField]
	private GameObject[] tWest = new GameObject[1];

	[Header("Pluses")]
	[SerializeField]
	private GameObject[] plus = new GameObject[1];

	//Generation Variables
	[Header("Generation Variables")]
	[SerializeField] [Tooltip("Make sure to plug in the TilePrefab object here from the MazeGen _Scripts folder")]
	private GameObject tilePrefab;

	[Space(10)]
	[SerializeField] [Tooltip("Standard width of our room prefabs")]
	private int roomWidth;
	[SerializeField] [Tooltip("Standard height of our room prefabs")]
	private int roomHeight;

	[Space(10)]
	[SerializeField] [Tooltip("Total width of the maze")]
	private int mazeWidth = 2;
	[SerializeField] [Tooltip("Total height of the maze")]
	private int mazeHeight = 2;

	[Space(10)]
	[SerializeField] [Tooltip("Pick a tile coordinate using x & y, and add an extra exit using z in the specified direction:\n" +
							  "0 = North\n" +
							  "1 = East\n" +
							  "2 = South\n" +
							  "3 = West")]
	private Vector3[] extraExits;

	private void Start()
	{
		//ensure maze has generation dimensions
		if (mazeWidth == 0 || mazeHeight == 0)
		{
			Debug.LogError("Set the dimensions, dummy");
			Debug.Break();
		}

		//start generation
		StartCoroutine(GenerateRooms());
	}

	//Maze generation adapted from u/DavoMyan on Reddit (and also myself from last semester)
	private List<MazeTile> visitedCells;
	private List<MazeTile> markedCells;
	private MazeTile currentCell;
	private bool finishedGeneration = false;
	private MazeTile[,] grid;
	void GenerateMaze()
	{
		//create a "Tile" array of maze width & height
		grid = new MazeTile[mazeWidth, mazeHeight];

		//populate the "Tile" array by instantiating tile objects & assigning them to each slot
		for (int ny = 0; ny < mazeWidth; ny++) {
			for (int nx = 0; nx < mazeHeight; nx++)
			{
				GameObject go = (GameObject)Instantiate(tilePrefab, transform.TransformPoint(new Vector3(nx*roomWidth, ny*roomHeight, 0)), Quaternion.identity, transform);
				MazeTile tile = go.GetComponent<MazeTile>();
				tile.x = nx; tile.y = ny;

				grid[nx, ny] = tile;
			}
		}

		//Bools for tile visitation during generation
		bool northVisited;
		bool eastVisited;
		bool southVisited;
		bool westVisited;

		//Lists to check off which tiles are visited, & which are marked to be visited soon
		visitedCells = new List<MazeTile>();
		markedCells = new List<MazeTile>();

		//Pick a tile, any tile!
		System.Random r = new System.Random();
		int x = Random.Range(0, mazeWidth);
		int y = Random.Range(0, mazeHeight);

		//Register chosen tile as current, and add it to visited
		currentCell = grid[x, y];
		currentCell.visited = true;
		visitedCells.Add(currentCell);

		//Here we go! (I think I made this a coroutine because it was breaking last I used it?)
		StartCoroutine(GenerateLogic());
		IEnumerator GenerateLogic()
		{
			do
			{
				x = currentCell.x;
				y = currentCell.y;

				//If tile is not on the left edge
				if (x != 0)
				{
					//If the tile to the left has not been marked or visited
					if (!grid[x - 1, y].marked && !grid[x - 1, y].visited)
					{
						//mark the tile to the left
						grid[x - 1, y].marked = true;
						markedCells.Add(grid[x - 1, y]);
					}
				}
				//If tile is not on right edge
				if (x != mazeWidth - 1)
				{
					//If the tile to the right has not been marked or visited
					if (!grid[x + 1, y].marked && !grid[x + 1, y].visited)
					{
						//mark the tile to the right
						grid[x + 1, y].marked = true;
						markedCells.Add(grid[x + 1, y]);
					}
				}
				//If tile is not on bottom edge
				if (y != 0)
				{
					//If the tile below has not been marked or visited
					if (!grid[x, y - 1].marked && !grid[x, y - 1].visited)
					{
						//mark the tile below
						grid[x, y - 1].marked = true;
						markedCells.Add(grid[x, y - 1]);
					}
				}
				//If tile is not on top edge
				if (y != mazeHeight - 1)
				{
					//If the tile above has not been marked or visited
					if (!grid[x, y + 1].marked && !grid[x, y + 1].visited)
					{
						//mark the tile above
						grid[x, y + 1].marked = true;
						markedCells.Add(grid[x, y + 1]);
					}
				}

				//Pick a random marked tile to become new current tile
				int rMarked = r.Next(markedCells.Count);
				currentCell = markedCells[rMarked];
				ResetVisited();

				////Debug stuff for generation visualization
				//Debug.Log(string.Format("Marked cells around ({0},{1})",x,y));
				//yield return new WaitForFixedUpdate();

				//Time to generate exits
				do
				{
					//yield return new WaitForFixedUpdate();

					//Pick a random direction
					int direction = Random.Range(0, 4);
					//Debug.Log("Visiting Adjacent, direction " + direction);

					//Based on picked direction:
					switch (direction)
					{
						//North
						case 0:
							//If the current tile is on top edge or the tile above hasn't been visited, try again
							if (currentCell.y == mazeHeight - 1 || !grid[currentCell.x, currentCell.y + 1].visited)
								break;
							else
							{
								//Open up north exit for current tile, and south exit for the tile above
								grid[currentCell.x, currentCell.y].exits[0] = 1;
								grid[currentCell.x, currentCell.y + 1].exits[2] = 1;

								//Add current tile to visted tiles, remove marking
								visitedCells.Add(grid[currentCell.x, currentCell.y]);
								grid[currentCell.x, currentCell.y].visited = true;
								markedCells.Remove(currentCell);

								//The North is clear!!
								northVisited = true;
							}
							break;
						//East
						case 1:
							//If the current tile is on right edge or the tile to the right hasn't been visited, try again
							if (currentCell.x == mazeWidth - 1 || !grid[currentCell.x + 1, currentCell.y].visited)
								break;
							else
							{
								//Open up east exit for current tile, and west exit for the tile to the right
								grid[currentCell.x, currentCell.y].exits[1] = 1;
								grid[currentCell.x + 1, currentCell.y].exits[3] = 1;

								//Add current tile to visited tiles, remove marking
								visitedCells.Add(grid[currentCell.x, currentCell.y]);
								grid[currentCell.x, currentCell.y].visited = true;
								markedCells.Remove(currentCell);

								//The East is clear!!
								eastVisited = true;
							}
							break;
						//South
						case 2:
							//If the current tile is on bottom edge or the tile below hasn't been visited, try again
							if (currentCell.y == 0 || !grid[currentCell.x, currentCell.y - 1].visited)
								break;
							else
							{
								//Open up south exit for current tile, and north exit for the tile below
								grid[currentCell.x, currentCell.y].exits[2] = 1;
								grid[currentCell.x, currentCell.y - 1].exits[0] = 1;

								//Add current tile to visited tiles, remove marking
								visitedCells.Add(grid[currentCell.x, currentCell.y]);
								grid[currentCell.x, currentCell.y].visited = true;
								markedCells.Remove(currentCell);

								//The South is clear!!
								southVisited = true;
							}
							break;
						//West
						case 3:
							//If the current tile is on left edge or the tile to the left hasn't been visited, try again
							if (currentCell.x == 0 || !grid[currentCell.x - 1, currentCell.y].visited)
								break;
							else
							{
								//Open up west exit for current tile, and east exit for the tile to the left
								grid[currentCell.x, currentCell.y].exits[3] = 1;
								grid[currentCell.x - 1, currentCell.y].exits[1] = 1;

								//Add current tile to visited tiles, remove marking
								visitedCells.Add(grid[currentCell.x, currentCell.y]);
								grid[currentCell.x, currentCell.y].visited = true;
								markedCells.Remove(currentCell);

								//The West is clear!!
								westVisited = true;
							}
							break;
						default:
							break;
					}

					//Loop this until any direction for this tile has been opened!
				} while (!northVisited && !eastVisited && !southVisited && !westVisited);

				//Loop this until all tiles have been visited!
			} while (visitedCells.Count < mazeWidth * mazeHeight);

			//Special cases for extra exits
			for (int i = 0; i < extraExits.Length; i++)
			{
				grid[(int)extraExits[i].x, (int)extraExits[i].y].exits[(int)extraExits[i].z] = 1;
			}

			//We're done generating the maze!
			finishedGeneration = true;
			yield return null;
		}

		//Making sure a new tile is ready for visitation
		void ResetVisited()
		{
			northVisited = false;
			eastVisited = false;
			southVisited = false;
			westVisited = false;
		}
	}

	//Generate Rooms
	IEnumerator GenerateRooms()
	{
		//Generate maze exits, do not proceed until this is done
		GenerateMaze();
		yield return new WaitUntil(() => finishedGeneration);

		foreach (MazeTile tile in grid)
		{
			GameObject roomPrefab = null;

			//Check which category a tile belongs in
			int exitSum = tile.exits[0] + tile.exits[1] + tile.exits[2] + tile.exits[3];
			if (exitSum == 2 && tile.exits[0] != tile.exits[2])
				exitSum = 5;

			//variable for picking random room from each set to place
			int r;

			//Caps
			if (exitSum == 1)
			{
				//North
				if (tile.exits[0] == 1)
				{
					r = Random.Range(0, capNorthExit.Length);
					roomPrefab = capNorthExit[r];
				}
				//East
				if (tile.exits[1] == 1)
				{
					r = Random.Range(0, capEastExit.Length);
					roomPrefab = capEastExit[r];
				}
				//South
				if (tile.exits[2] == 1)
				{
					r = Random.Range(0, capSouthExit.Length);
					roomPrefab = capSouthExit[r];
				}
				//West
				if (tile.exits[3] == 1)
				{
					r = Random.Range(0, capWestExit.Length);
					roomPrefab = capWestExit[r];
				}
			}

			//Halls
			if (exitSum == 2)
			{
				//Vertical
				if (tile.exits[0] == 1)
				{
					r = Random.Range(0, hallVertical.Length);
					roomPrefab = hallVertical[r];
				}
				//Horizontal
				if (tile.exits[1] == 1)
				{
					r = Random.Range(0, hallHorizontal.Length);
					roomPrefab = hallHorizontal[r];
				}
			}

			//Hooks
			if (exitSum == 5)
			{
				//NorthEast
				if (tile.exits[0] == 1 && tile.exits[1] == 1)
				{
					r = Random.Range(0, hookNorthEast.Length);
					roomPrefab = hookNorthEast[r];
				}
				//SouthEast
				if (tile.exits[1] == 1 && tile.exits[2] == 1)
				{
					r = Random.Range(0, hookSouthEast.Length);
					roomPrefab = hookSouthEast[r];
				}
				//SouthWest
				if (tile.exits[2] == 1 && tile.exits[3] == 1)
				{
					r = Random.Range(0, hookSouthWest.Length);
					roomPrefab = hookSouthWest[r];
				}
				//NorthWest
				if (tile.exits[3] == 1 && tile.exits[0] == 1)
				{
					r = Random.Range(0, hookNorthWest.Length);
					roomPrefab = hookNorthWest[r];
				}
			}

			//T-Shapes
			if (exitSum == 3)
			{
				//North
				if (tile.exits[0] == 1 && tile.exits[1] == 1 && tile.exits[3] == 1)
				{
					r = Random.Range(0, tNorth.Length);
					roomPrefab = tNorth[r];
				}
				//East
				if (tile.exits[1] == 1 && tile.exits[2] == 1 && tile.exits[0] == 1)
				{
					r = Random.Range(0, tEast.Length);
					roomPrefab = tEast[r];
				}
				//South
				if (tile.exits[2] == 1 && tile.exits[3] == 1 && tile.exits[1] == 1)
				{
					r = Random.Range(0, tSouth.Length);
					roomPrefab = tSouth[r];
				}
				//West
				if (tile.exits[3] == 1 && tile.exits[0] == 1 && tile.exits[2] == 1)
				{
					r = Random.Range(0, tWest.Length);
					roomPrefab = tWest[r];
				}
			}

			//Pluses
			if (exitSum == 4)
			{
				r = Random.Range(0, plus.Length);
				roomPrefab = plus[r];
			}

			GameObject go = (GameObject)Instantiate(roomPrefab, transform.TransformPoint(new Vector3(tile.x*roomWidth, tile.y*roomHeight, 0)), Quaternion.identity, transform);
			go.name = string.Format("({0},{1})", tile.x, tile.y);
			
			Destroy(tile.gameObject);
		}
	}
}
