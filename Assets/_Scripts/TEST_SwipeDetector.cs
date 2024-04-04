using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TEST_SwipeDetector : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private BoxCollider2D _collider;
	[SerializeField] private Vector2 _dimensions;

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
				{
					if (_text != null)
						_text.text = "Right";
					if (_dimensions != Vector2.zero)
						transform.position = new Vector3(transform.position.x + _dimensions.x, transform.position.y, transform.position.z);
				}
				if (swipeDirection.x < 0)
				{
					if (_text != null)
						_text.text = "Left";
					if (_dimensions != Vector2.zero)
						transform.position = new Vector3(transform.position.x - _dimensions.x, transform.position.y, transform.position.z);
				}
			}

			//Check vertical direction
			if (Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x))
			{
				if (swipeDirection.y > 0)
				{
					if (_text != null)
						_text.text = "Up";
					if (_dimensions != Vector2.zero)
						transform.position = new Vector3(transform.position.x, transform.position.y + _dimensions.y, transform.position.z);
				}
				if (swipeDirection.y < 0)
				{
					if (_text != null)
						_text.text = "Down";
					if (_dimensions != Vector2.zero)
						transform.position = new Vector3(transform.position.x, transform.position.y - _dimensions.y, transform.position.z);
				}
			}
	}
}

}
