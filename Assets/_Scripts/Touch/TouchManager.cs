using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Events;

public class TouchManager : MonoBehaviour
{
	[Header("Input Controls")]
	[SerializeField] private bool enableTap = true;
	[SerializeField] private bool enableSwipe = true;

	private enum TapBehavior { tapBinding, tapInteraction };

	[Header("Input Settings")]
	[SerializeField] private TapBehavior tapBehavior = TapBehavior.tapInteraction;
	[SerializeField] private float minimumSwipeMagnitude = 10f;
	private Vector2 _swipeDirection;
	private Vector2 _swipeStartpos;

    public static TouchManager Instance { get; private set; }
    TouchAction touchAction;
    public UnityAction<Vector2> OnTap;
	public UnityAction<Vector2, Vector2> OnSwipe;

    private void Awake()
    {
        touchAction = new TouchAction();
        touchAction.Enable();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
		//Taps
		if (enableTap)
		{
			if (tapBehavior == TapBehavior.tapBinding)
			{
				touchAction.Gameplay.TapBinding.started += ctx => TapStarted(ctx);
				touchAction.Gameplay.TapBinding.performed += ctx => TapPerformed(ctx);
				touchAction.Gameplay.TapBinding.canceled += ctx => TapEnd(ctx);
			}
			if (tapBehavior == TapBehavior.tapInteraction)
			{
				touchAction.Gameplay.TapInteraction.started += ctx => TapStarted(ctx);
				touchAction.Gameplay.TapInteraction.performed += ctx => TapPerformed(ctx);
				touchAction.Gameplay.TapInteraction.canceled += ctx => TapEnd(ctx);
			}
		}

		//Swipes
		if (enableSwipe)
		{
			touchAction.Gameplay.Touch.started += ctx => TouchStarted(ctx);
			touchAction.Gameplay.Swipe.performed += ctx => SwipePerformed(ctx);
			touchAction.Gameplay.Touch.canceled += ctx => TouchCanceled(ctx);
		}
	}

    private void OnDisable()
    {
		//Taps
		if (enableTap)
		{
			if (tapBehavior == TapBehavior.tapBinding)
			{
				touchAction.Gameplay.TapBinding.started -= ctx => TapStarted(ctx);
				touchAction.Gameplay.TapBinding.performed -= ctx => TapPerformed(ctx);
				touchAction.Gameplay.TapBinding.canceled -= ctx => TapEnd(ctx);
			}
			if (tapBehavior == TapBehavior.tapInteraction)
			{
				touchAction.Gameplay.TapInteraction.started -= ctx => TapStarted(ctx);
				touchAction.Gameplay.TapInteraction.performed -= ctx => TapPerformed(ctx);
				touchAction.Gameplay.TapInteraction.canceled -= ctx => TapEnd(ctx);
			}
		}

		//Swipes
		if (enableSwipe)
		{
			touchAction.Gameplay.Touch.started -= ctx => TouchStarted(ctx);
			touchAction.Gameplay.Swipe.performed -= ctx => SwipePerformed(ctx);
			touchAction.Gameplay.Touch.canceled -= ctx => TouchCanceled(ctx);
		}
	}

	//Taps
    private void TapStarted(InputAction.CallbackContext ctx)
	{
		// Debug.Log("Tap Started.");
    }

    private void TapPerformed(InputAction.CallbackContext ctx)
    {
		//Do not perform tap if swiping
		if (Mathf.Abs(_swipeDirection.magnitude) >= minimumSwipeMagnitude)
			return;

		//Get tap position
		var touchPosition = touchAction.Gameplay.TouchPosition.ReadValue<Vector2>();
		// Debug.Log("Tap Position: " + touchPosition);

		//Call out for tap event
		OnTap?.Invoke(touchPosition);

		// Debug.Log("Tap Performed.");
	}

    private void TapEnd(InputAction.CallbackContext ctx)
    {
		// Debug.Log("Tap Canceled.");
	}


	//Swipes
	private void TouchStarted(InputAction.CallbackContext ctx)
	{
		//Get start position
		_swipeStartpos = touchAction.Gameplay.TouchPosition.ReadValue<Vector2>();

		if (!enableTap)
			Debug.Log("Touch Position: " + _swipeStartpos);
	}

	private void SwipePerformed(InputAction.CallbackContext ctx)
	{
		//Get direction
		_swipeDirection = ctx.ReadValue<Vector2>();
	}

	private void TouchCanceled(InputAction.CallbackContext ctx)
	{
		if (!enableTap)
			Debug.Log("Touch Complete");

		//Check magnitude for swipe
		if (Mathf.Abs(_swipeDirection.magnitude) < minimumSwipeMagnitude)
			return;
		Debug.Log("Swipe Detected");

		//Call out for swipe event
		OnSwipe?.Invoke(_swipeDirection, _swipeStartpos);

		//reset variables
		_swipeDirection = Vector2.zero;
		_swipeStartpos = Vector2.zero;
	}
}
