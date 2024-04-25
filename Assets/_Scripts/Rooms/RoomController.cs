using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
	[SerializeField]
	private GameObject objectsContainer;
	[SerializeField]
	private List<GameObject> initialObjectList;

	[Space(10)]
	[SerializeField]
	private bool excludeChests;

	private GameObject objectClones;

	[Space(10)]
	[SerializeField]
	private bool[] exits = new bool[4];
	[SerializeField]
	private GameObject exitDoorPrefab;
	private GameObject[] doors = new GameObject[4];
	[SerializeField]
	private bool neverOpen;

	private void Awake()
	{
		Debug.Log(this.gameObject.name + " is creating a cloned objects container");
		objectClones = new GameObject("CloneObjects");
		objectClones.transform.parent = transform;
		objectClones.transform.position = Vector2.zero;

		for (int i = 0; i < objectsContainer.transform.childCount; i++)
		{
			GameObject go = objectsContainer.transform.GetChild(i).gameObject;

			if (excludeChests && go.GetComponent<ChestManager>()) {}
			else
			{
				initialObjectList.Add(go);
				Instantiate(go, objectClones.transform);
				go.SetActive(false);
			}
		}
	}

	private void OnEnable()
	{
		CombatManager.Instance.OnTurnChanged += OnTurnChanged;
	}

	private void OnDisable()
	{
		CombatManager.Instance.OnTurnChanged -= OnTurnChanged;
	}

	[ContextMenu("Reset")]
	public void ResetRoom()
	{
		Destroy(objectClones);
		objectClones = new GameObject("CloneObjects");
		objectClones.transform.parent = transform;
		objectClones.transform.position = Vector2.zero;

		for (int i = 0; i < initialObjectList.Count; i++)
		{
			Instantiate(initialObjectList[i], objectClones.transform).SetActive(true);
		}
	}

	[ContextMenu("Lock Doors")]
	public void LockDoors()
	{
		MazeGen maze = MazeGen.Instance;

		if (exits[0])
		{
			doors[0] = Instantiate(exitDoorPrefab, transform);
			doors[0].transform.localPosition = new Vector2(0, (maze.roomHeight/2)-0.5f);
		}

		if (exits[1])
		{
			doors[1] = Instantiate(exitDoorPrefab, transform);
			doors[1].transform.localPosition = new Vector2((maze.roomWidth/2)-0.5f, 0);
			doors[1].transform.rotation = Quaternion.Euler(0, 0, 90);
		}

		if (exits[2])
		{
			doors[2] = Instantiate(exitDoorPrefab, transform);
			doors[2].transform.localPosition = new Vector2(0, (-maze.roomHeight/2)+0.5f);
		}

		if (exits[3])
		{
			doors[3] = Instantiate(exitDoorPrefab, transform);
			doors[3].transform.localPosition = new Vector2((-maze.roomWidth/2)+0.5f, 0);
			doors[3].transform.rotation = Quaternion.Euler(0, 0, 90);
		}
	}

	[ContextMenu("Unlock Doors")]
	public void UnlockDoors()
	{
		if (neverOpen) return;

		for (int i = 0; i < doors.Length; i++)
		{
			Destroy(doors[i]);
		}
	}

	void OnTurnChanged(CombatTurnState turn)
	{
		if (turn == CombatTurnState.NonCombat)
			UnlockDoors();
	}
}
