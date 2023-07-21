using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Godmode : MonoBehaviour, GameplayInput.IGodmodeActions
{
    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();
    private RollController cached_RC;
    private RollController RC => cached_RC ??= GetComponent<RollController>();

    [SerializeField] private float speed;
    Vector3 RawMovementInput;

    private Vector3 WorldMovementDirection => (Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * RawMovementInput).normalized;

    [SerializeField] private Transform cameraTransform;
    GameplayInput input;

    void Start()
    {
        input = new();
        input.Godmode.SetCallbacks(this);
        input.Godmode.Enable();
    }

    void OnDestroy()
    {
        input.Dispose();
    }

    public void OnMovementDirection(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector3>();
    }


    public static bool hasBeenUsed;
    bool gmEnabled = false;
    public void OnToggle(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        gmEnabled = !gmEnabled;
        RC.enabled = !gmEnabled;
        RB.isKinematic = gmEnabled;                
        hasBeenUsed = true;
        LevelTimer.Cancel();
    }

    void FixedUpdate()
    {
        if (!gmEnabled) return;
        RB.MovePosition(RB.position + WorldMovementDirection * speed * Time.fixedDeltaTime);
    }
}
