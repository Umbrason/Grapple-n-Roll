using System;

public interface ISignalEmitter
{
    event Action<bool> OnStateChanged;
}
