
using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuCanvas;
    private Guid pauseHandle;
    private bool selfPaused;
    public void TogglePause()
    {
        if (!selfPaused && PauseManager.Paused) return;
        selfPaused = !selfPaused;
        PauseMenuCanvas?.SetActive(selfPaused);
        if (selfPaused) pauseHandle = PauseManager.Pause();
        if (!selfPaused) PauseManager.Resume(pauseHandle);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }



    void OnDestroy()
    {
        if (selfPaused) PauseManager.Resume(pauseHandle);
    }

}
