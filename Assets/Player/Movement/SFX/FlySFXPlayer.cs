
using UnityEngine;

public class FlySFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private CollisionInfo CI;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private AnimationCurve volumeCurve;

    private float volumeChangeTime = .1f;

    void Update()
    {
        var volumeTarget = RB.velocity.magnitude;
        volumeTarget *= Time.timeScale;
        volumeTarget = volumeCurve.Evaluate(volumeTarget);
        var volume = Mathf.MoveTowards(SFXSource.volume, volumeTarget, 1f / volumeChangeTime * Time.unscaledDeltaTime);
        SFXSource.volume = volume;
        SFXSource.enabled = volume > 0;
    }
}
