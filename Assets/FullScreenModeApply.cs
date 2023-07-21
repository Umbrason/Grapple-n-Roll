
using UnityEngine;

public class FullScreenModeApply : MonoBehaviour
{
    void Start()
    {
        Screen.fullScreenMode = PlayerOptions.FullScreenMode;
        PlayerOptions.OnFullScreenModeChanged += SetFullScreenMode;
    }

    void OnDestroy()
    {
        PlayerOptions.OnFullScreenModeChanged -= SetFullScreenMode;
    }

    void SetFullScreenMode(FullScreenMode fullScreenMode)
    {
        Screen.fullScreenMode = fullScreenMode;
    }
}
