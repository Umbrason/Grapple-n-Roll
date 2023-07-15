
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private SignalEmitterComponent emitter;
    [SerializeField] private Transform DoorTransform;
    void OnEnable()
    {
        emitter.OnStateChanged += OnSignalChanged;
    }

    void OnDisable()
    {
        emitter.OnStateChanged -= OnSignalChanged;
    }

    [SerializeField] private Vector3 openDirection = Vector3.down * 5f;
    [SerializeField] private float transitionDuration;
    [SerializeField] private AnimationCurve transitionAnimation;

    private float targetOpenness = 0;
    private float currentOpenness = 0;
    void FixedUpdate()
    {
        currentOpenness = Mathf.MoveTowards(currentOpenness, targetOpenness, Time.fixedDeltaTime / transitionDuration);
        var openAmplitude = transitionAnimation.Evaluate(currentOpenness);
        DoorTransform.localPosition = openDirection * openAmplitude;
    }

    void OnSignalChanged(bool signal)
    {
        targetOpenness = signal ? 1 : 0;
    }
}
