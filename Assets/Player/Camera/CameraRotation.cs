using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    void Start()
    {
        RollingInputEvents.OnTurnCameraChanged += TurnCamera;
        PauseManager.OnPause += OnPause;
        PauseManager.OnResume += OnResume;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDestroy()
    {
        RollingInputEvents.OnTurnCameraChanged -= TurnCamera;
        Cursor.lockState = CursorLockMode.None;
        PauseManager.OnPause -= OnPause;
        PauseManager.OnResume -= OnResume;
    }

    private void OnPause() => Cursor.lockState = CursorLockMode.None;
    private void OnResume() => Cursor.lockState = CursorLockMode.Locked;

    private float rx = 0f;
    private float ry = 0f;
    private bool firstInput = true;
    private void TurnCamera(Vector2 Delta)
    {
        if (firstInput)
        {
            firstInput = false;
            return;
        }
        rx = Mathf.Clamp(rx - Delta.y * PlayerOptions.Sensitivity / 20f * Time.timeScale, -90, 90);
        ry += Delta.x * PlayerOptions.Sensitivity / 20f * Time.timeScale;
        ry %= 360f;
        transform.localRotation = Quaternion.Euler(rx, ry, 0);
    }

}
