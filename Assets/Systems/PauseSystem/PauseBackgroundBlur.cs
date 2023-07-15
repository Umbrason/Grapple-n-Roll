using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseBackgroundBlur : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration = .3f;
    [SerializeField] private AnimationCurve fadeOutCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private Volume DOFVolume;

    private const float FocalLengthMin = 1;
    private const float FocalLengthMax = 50;

    void OnEnable()
    {
        PauseManager.OnPause += Pause;
        PauseManager.OnResume += Resume;
    }

    void OnDisable()
    {
        PauseManager.OnPause -= Pause;
        PauseManager.OnResume -= Resume;
    }

    private void Pause()
    {
        PauseStartTime = Time.unscaledTime;
        DOFVolume.enabled = true;
    }

    private void Resume()
    {
        PauseStartTime = float.MaxValue;
        DOFVolume.enabled = false;
    }

    private float PauseStartTime = float.MaxValue;
    void Update()
    {
        var t = Time.unscaledTime - PauseStartTime;
        t /= fadeOutDuration;
        if (t < 0 || t >= 1) return;
        var curveT = fadeOutCurve.Evaluate(t);
        if (DOFVolume.profile.TryGet<DepthOfField>(out var DOF)) DOF.focalLength.value = Mathf.Lerp(FocalLengthMin, FocalLengthMax, curveT);        
    }
}
