
using UnityEngine;

public class PauseUnlocksCursor : MonoBehaviour
{
    void OnEnable()
    {
        PauseManager.OnPause += Pause;
        PauseManager.OnResume += Resume;
    }

    void OnDisable()
    {
        PauseManager.OnPause -= Pause;
        PauseManager.OnResume -= Resume;
        Resume();
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

}
