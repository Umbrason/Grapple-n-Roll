using System;
using UnityEngine;

public static class LevelTimer
{
    public static event Action<float> OnStart;
    public static event Action<float> OnFinish;

    private static float StartTime;
    private static float StopTime;
    public static float CurrentTime => Time.unscaledTime - StartTime;

    public static void StartTimer()
    {
        StartTime = Time.unscaledTime;
        OnStart?.Invoke(StartTime);
    }

    public static void StopTimer()
    {
        StopTime = Time.unscaledTime;
        OnFinish?.Invoke(CurrentTime);
    }
}
