
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider volumeSlider;

    void OnEnable()
    {
        nameInput.onSubmit.AddListener(ChangeName);
        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        nameInput.SetTextWithoutNotify(PlayerOptions.DisplayName);
        sensitivitySlider.value = PlayerOptions.Sensitivity;
        volumeSlider.value = PlayerOptions.Volume;
    }

    void OnDisable()
    {
        nameInput.onSubmit.RemoveListener(ChangeName);
        sensitivitySlider.onValueChanged.RemoveListener(ChangeSensitivity);
        volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
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
