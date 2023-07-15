
using System;
using UnityEngine;

public class PressurePlate : SignalEmitterComponent
{
    [SerializeField] private bool staysOn;
    [SerializeField] private Transform pressurePlateVisual;
    [SerializeField] private float pressurePlateDepth = .3f;

    private int contactCount;
    public void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody == null) return;
        if (++contactCount > 1) return;
        ChangeState(true);
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
        pressurePlateVisual.localPosition = state ? pressurePlateDepth * Vector3.down : Vector3.zero;
    }
}
