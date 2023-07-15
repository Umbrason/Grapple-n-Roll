
using System;
using UnityEngine;

public class PauseOnFinish : MonoBehaviour
{
    void Start()
    {
        LevelTimer.OnFinish += Pause;
    }

    void OnDestroy()
    {
        LevelTimer.OnFinish -= Pause;
    }

    private Guid pauseHandle;
    void Pause(float _)
    {
        pauseHandle = PauseManager.Pause();
    }

    void Resume()
    {
        PauseManager.Resume(pauseHandle);
    }
}
