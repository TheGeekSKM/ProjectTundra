using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TEST_SwipeDetector : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private BoxCollider2D _collider;

	private void OnEnable()
	{
		TouchManager.Instance.OnSwipe += OnSwipe;
	}

	private void OnDisable()
	{
		TouchManager.Instance.OnSwipe -= OnSwipe;
	}

	private void OnSwipe(Vector2 swipeDirection, Vector2 swipeStartPos)
	{
		//Check if swipe started on this box
		var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(swipeStartPos.x, swipeStartPos.y, 10));
		var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
		if (hit.collider == this._collider)
		{
			Debug.Log("Swipe started in box!");

			//Check horizontal direction
			if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
			{
				if (swipeDirection.x > 0)
					_text.text = "Right";
				if (swipeDirection.x < 0)
					_text.text = "Left";
			}

			//Check vertical direction
			if (Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x))
			{
				if (swipeDirection.y > 0)
					_text.text = "Up";
				if (swipeDirection.y < 0)
					_text.text = "Down";
			}
	}
}

}
