
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollingInputEvents : MonoBehaviour, GameplayInput.IRollingActions
{
    GameplayInput input;
    void Start()
    {
        input = new();
        input.Rolling.SetCallbacks(this);
        input.Rolling.Enable();
    }

    void OnDestroy()
    {
        input.Dispose();
    }

    public static event Action<Vector2> OnRollDirectionChanged;
    public void OnRollDirection(InputAction.CallbackContext context)
    {
        OnRollDirectionChanged.Invoke(context.ReadValue<Vector2>());
    }

    public static event Action<Vector2> OnTurnCameraChanged;
    public void OnTurnCamera(InputAction.CallbackContext context)
    {
        OnTurnCameraChanged.Invoke(context.ReadValue<Vector2>());
    }
}
