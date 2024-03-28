using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour
{
	public int x;
	public int y;
	public int[] exits = new int[4];
	public bool visited;
	public bool marked;
}
