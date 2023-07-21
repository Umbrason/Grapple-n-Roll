
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle[] windowModeToggles;

    void OnEnable()
    {
        nameInput.onSubmit.AddListener(ChangeName);
        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        nameInput.SetTextWithoutNotify(PlayerOptions.DisplayName);
        sensitivitySlider.value = PlayerOptions.Sensitivity;
        volumeSlider.value = PlayerOptions.Volume;
        for (int i = 0; i < modes.Length; i++)
        {
            windowModeToggles[i].SetIsOnWithoutNotify(modes[i] == PlayerOptions.FullScreenMode);
            windowModeToggles[i].onValueChanged.RemoveAllListeners();
            var windowMode = modes[i];
            windowModeToggles[i].onValueChanged.AddListener(value => { if (value) ChangeWindowMode(windowMode); });
        }
    }

    static readonly FullScreenMode[] modes = new FullScreenMode[]
    {
        FullScreenMode.MaximizedWindow,
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.Windowed
    };

    void OnDisable()
    {
        nameInput.onSubmit.RemoveListener(ChangeName);
        sensitivitySlider.onValueChanged.RemoveListener(ChangeSensitivity);
        volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
    }

    void ChangeWindowMode(FullScreenMode windowMode)
    {
        PlayerOptions.FullScreenMode = windowMode;
    }

    void ChangeName(string name)
    {
        PlayerOptions.DisplayName = name;
    }

    void ChangeSensitivity(float sensitivity)
    {
        PlayerOptions.Sensitivity = sensitivity;
    }

    void ChangeVolume(float volume)
    {
        PlayerOptions.Volume = volume;
    }
}
