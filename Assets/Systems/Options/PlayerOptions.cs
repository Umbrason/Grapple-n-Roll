using System;
using Unity.Services.Authentication;
using UnityEngine;

public static class PlayerOptions
{
    private static string cached_DisplayName;
    public static string DisplayName
    {
        get => cached_DisplayName ??= PlayerPrefs.GetString(nameof(DisplayName), AuthenticationService.Instance.PlayerName);
        set
        {
            PlayerPrefs.SetString(nameof(DisplayName), cached_DisplayName = value);
            OnNameChanged?.Invoke(value);
        }
    }

    private static float? cached_Volume;
    public static float Volume
    {
        get => cached_Volume ??= PlayerPrefs.GetFloat(nameof(Volume), 0f);
        set
        {
            PlayerPrefs.SetFloat(nameof(Volume), (cached_Volume = value) ?? 0);
            OnVolumeChanged?.Invoke(value);
        }

    }

    private static float? cached_Sensitivity;
    public static float Sensitivity
    {
        get => cached_Sensitivity ??= PlayerPrefs.GetFloat(nameof(Sensitivity), 1f);
        set
        {
            PlayerPrefs.SetFloat(nameof(Sensitivity), (cached_Sensitivity = value) ?? 0);
            OnSensitivityChanged?.Invoke(value);
        }
    }

    private static FullScreenMode? cached_FullScreenMode;
    public static FullScreenMode FullScreenMode
    {
        get => cached_FullScreenMode ??= (FullScreenMode)PlayerPrefs.GetInt(nameof(FullScreenMode), (int)FullScreenMode.MaximizedWindow);
        set
        {
            PlayerPrefs.SetInt(nameof(FullScreenMode), (int?)(cached_FullScreenMode = value) ?? (int)FullScreenMode.MaximizedWindow);
            Screen.fullScreenMode = value;
            OnFullScreenModeChanged?.Invoke(value);
        }
    }

    

    public static Action<string> OnNameChanged;
    public static Action<float> OnVolumeChanged;
    public static Action<float> OnSensitivityChanged;
    public static Action<FullScreenMode> OnFullScreenModeChanged;

}