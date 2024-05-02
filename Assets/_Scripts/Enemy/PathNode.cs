using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
	public Vector2 position;
	public int g_cost;
	public int h_cost;
	public int f_cost;
	public bool walkable = true;
	public bool start;
	public bool end;
}
