using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Events;

public class TouchManager : MonoBehaviour
{
	private enum TapBehavor { tapBinding, tapInteraction };
	[SerializeField] private TapBehavor tapBehavor = TapBehavor.tapBinding;

    public static TouchManager Instance { get; private set; }
    TouchAction touchAction;
    public UnityAction<Vector2> OnTouchStarted;

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
		if (tapBehavor == TapBehavor.tapBinding)
		{
			touchAction.Gameplay.TapBinding.started += ctx => TapStarted(ctx);
			touchAction.Gameplay.TapBinding.performed += ctx => TapPerformed(ctx);
			touchAction.Gameplay.TapBinding.canceled += ctx => TapEnd(ctx);
		}
		if (tapBehavor == TapBehavor.tapInteraction)
		{
			touchAction.Gameplay.TapInteraction.started += ctx => TapStarted(ctx);
			touchAction.Gameplay.TapInteraction.performed += ctx => TapPerformed(ctx);
			touchAction.Gameplay.TapInteraction.canceled += ctx => TapEnd(ctx);
		}
	}

    private void OnDisable()
    {
		if (tapBehavor == TapBehavor.tapBinding)
		{
			touchAction.Gameplay.TapBinding.started -= ctx => TapStarted(ctx);
			touchAction.Gameplay.TapBinding.performed -= ctx => TapPerformed(ctx);
			touchAction.Gameplay.TapBinding.canceled -= ctx => TapEnd(ctx);
		}
		if (tapBehavor == TapBehavor.tapInteraction)
		{
			touchAction.Gameplay.TapInteraction.started -= ctx => TapStarted(ctx);
			touchAction.Gameplay.TapInteraction.performed -= ctx => TapPerformed(ctx);
			touchAction.Gameplay.TapInteraction.canceled -= ctx => TapEnd(ctx);
		}
	}

    private void TapStarted(InputAction.CallbackContext ctx)
	{
		Debug.Log("Touch Started.");
    }

    private void TapPerformed(InputAction.CallbackContext ctx)
    {
		var touchPosition = touchAction.Gameplay.TouchPosition.ReadValue<Vector2>();
		Debug.Log("Touch Position: " + touchPosition);
		OnTouchStarted?.Invoke(touchPosition);

		Debug.Log("Touch Performed.");
	}

    private void TapEnd(InputAction.CallbackContext ctx)
    {
		Debug.Log("Touch Canceled.");
	}

}
