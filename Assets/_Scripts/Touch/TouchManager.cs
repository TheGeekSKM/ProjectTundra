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
    public UnityAction<Vector2> OnTouchPerformed;

    private void Awake()
    {
        touchAction = new TouchAction();
        touchAction.Gameplay.Enable();

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
        touchAction.Gameplay.Touch.started += ctx => TouchPerformed(ctx);
        touchAction.Gameplay.Touch.canceled += ctx => TouchEnd(ctx);
    }

    private void OnDisable()
    {
        touchAction.Gameplay.Touch.started -= ctx => TouchPerformed(ctx);
        touchAction.Gameplay.Touch.canceled -= ctx => TouchEnd(ctx);
    }

    private void TouchPerformed(InputAction.CallbackContext ctx)
    {
        var touchPosition = ctx.ReadValue<TouchState>().position;
        Debug.Log("Touch Position: " + touchPosition);
        OnTouchPerformed?.Invoke(touchPosition);
    }

    private void TouchEnd(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch End");
    }

}
