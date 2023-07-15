using System;
using UnityEngine;

public abstract class SignalEmitterComponent : MonoBehaviour, ISignalEmitter
{
    public event Action<bool> OnStateChanged;
    protected void RaiseStateChangeEvent(bool state) => OnStateChanged?.Invoke(state);
}