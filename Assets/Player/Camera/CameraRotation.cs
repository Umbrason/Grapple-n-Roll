using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    void Start()
    {
        RollingInputEvents.OnTurnCameraChanged += TurnCamera;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Destroy()
    {
        RollingInputEvents.OnTurnCameraChanged -= TurnCamera;
        Cursor.lockState = CursorLockMode.None;
    }

    [SerializeField] private float Sensitivity = 5f;
    private float rx = 0f;
    private float ry = 0f;
    private void TurnCamera(Vector2 Delta)
    {
        rx = Mathf.Clamp(rx - Delta.y * Sensitivity / 20f, -90, 90);
        ry += Delta.x * Sensitivity / 20f;
        ry %= 360f;
        transform.localRotation = Quaternion.Euler(rx, ry, 0);
    }

}
