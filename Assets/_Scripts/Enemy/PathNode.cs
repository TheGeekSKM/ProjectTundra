using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
	public Vector2 position;
	public Vector2Int gridPos;
	public int g_cost;
	public int h_cost;
	public bool walkable = true;
	public bool start;
	public bool end;
	public PathNode parent;

	public int f_cost
	{
		get
		{
			return g_cost + h_cost;
		}
	}
}
