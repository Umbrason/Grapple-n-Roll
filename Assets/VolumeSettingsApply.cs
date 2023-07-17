
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettingsApply : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixer;
    void OnEnable()
    {
        PlayerOptions.OnVolumeChanged += ChangeVolume;
        ChangeVolume(PlayerOptions.Volume);
    }

    void OnDisable()
    {
        PlayerOptions.OnVolumeChanged -= ChangeVolume;
    }

    void ChangeVolume(float v)
    {
        mixer.audioMixer.SetFloat("Volume", v);
    }
}

