using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float duration;

    public event System.Action OnCameraMovementFinish;

    public void Move(Vector2 direction)
    {
		//Get Maze
		MazeGen maze = MazeGen.Instance;

		//Set room destination based on direction inputted
		Vector3 destination = transform.position;
		destination.x += direction.x * maze.roomWidth;
		destination.y += direction.y * maze.roomHeight;

		//Start movement routine
        StartCoroutine(Movement(destination));
    }
    IEnumerator Movement(Vector3 destination)
    {
		//Do the tween and wait for duration to elapse
        transform.DOMove(destination, duration);
        yield return new WaitForSeconds(duration);

		//Call out finished event
        OnCameraMovementFinish.Invoke();
        yield break;
    }

}
