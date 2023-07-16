
using System;
using UnityEngine;

public class PressurePlate : SignalEmitterComponent
{
    [SerializeField] private bool staysOn;
    [SerializeField] private Transform pressurePlateVisual;
    [SerializeField] private float pressurePlateDepth = .3f;
    [SerializeField] private AudioClip sfxActivate;
    [SerializeField] private AudioClip sfxDeactivate;
    [SerializeField] private AudioSource sfxSource;

    private int contactCount;
    private bool wasOnBefore;
    public void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody == null) return;
        if (++contactCount > 1) return;
        if (wasOnBefore && staysOn) return;
        ChangeState(true);
        wasOnBefore = true;
    }

    public void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody == null) return;
        if (--contactCount > 0 || staysOn) return;
        ChangeState(false);
    }

    private void ChangeState(bool state)
    {
        RaiseStateChangeEvent(state);
        sfxSource.PlayOneShot(state ? sfxActivate : sfxDeactivate);
        pressurePlateVisual.localPosition = state ? pressurePlateDepth * Vector3.down : Vector3.zero;
    }
}