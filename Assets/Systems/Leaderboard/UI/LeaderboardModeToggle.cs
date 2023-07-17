
using UnityEngine;

public class LeaderboardModeToggle : MonoBehaviour
{
    [SerializeField] private LeaderboardUI LBUI;

    [SerializeField] private LeaderboardUI.Mode mode;
    public void OnValueChanged(bool value)
    {
        if (!value) return;
        LBUI.SetMode(mode);
    }

}
