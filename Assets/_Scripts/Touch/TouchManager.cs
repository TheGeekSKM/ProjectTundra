using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Events;

public class TouchManager : MonoBehaviour
{
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
        touchAction.Gameplay.TouchPosition.started += ctx => TouchStarted(ctx);
        touchAction.Gameplay.TouchPosition.canceled += ctx => TouchEnd(ctx);
    }

    private void OnDisable()
    {
        touchAction.Gameplay.TouchPosition.started -= ctx => TouchStarted(ctx);
        touchAction.Gameplay.TouchPosition.canceled -= ctx => TouchEnd(ctx);
    }

    private void TouchStarted(InputAction.CallbackContext ctx)
    {
        var touchPosition = ctx.ReadValue<Vector2>();
        Debug.Log("Touch Position: " + touchPosition);
        OnTouchStarted?.Invoke(touchPosition);
    }

    private void TouchPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch Performed");
    }

    private void TouchEnd(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch End");
    }

}
