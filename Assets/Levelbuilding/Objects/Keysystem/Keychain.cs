
using System;
using UnityEngine;

public class Keychain : MonoBehaviour
{
    public int KeyCount { get; private set; }
    public event Action<int> OnKeyCountChanged;
    public void Collect()
    {
        OnKeyCountChanged.Invoke(++KeyCount);
    }

}
