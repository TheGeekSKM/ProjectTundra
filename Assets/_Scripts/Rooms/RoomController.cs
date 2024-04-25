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

	private void Start()
	{
		objectClones = Instantiate(new GameObject("Objects"), transform);

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

	[ContextMenu("Reset")]
	public void ResetRoom()
	{
		Destroy(objectClones);
		objectClones = Instantiate(new GameObject("Objects"), transform);

		for (int i = 0; i < initialObjectList.Count; i++)
		{
			Instantiate(initialObjectList[i], objectClones.transform).SetActive(true);
		}
	}
}
