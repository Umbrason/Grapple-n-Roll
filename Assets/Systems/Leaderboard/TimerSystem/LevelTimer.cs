using System;
using UnityEngine;

public static class LevelTimer
{
    public static event Action<float> OnStart;
    public static event Action<float> OnFinish;

    private static float StartTime;
    private static float StopTime;
    public static bool Running => m_Running;
    private static bool m_Running = false;
    public static float CurrentTime => Time.fixedTime - StartTime;

    public static void StartTimer()
    {
        if (m_Running) return;
        StartTime = Time.fixedTime;
        m_Running = true;
        OnStart?.Invoke(StartTime);
    }

    public static void StopTimer()
    {
        if (!m_Running) return;
        StopTime = Time.fixedTime;
        m_Running = false;
        OnFinish?.Invoke(CurrentTime);
    }
}
