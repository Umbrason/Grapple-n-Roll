
using UnityEngine;

public class TimedObject : MonoBehaviour
{
    void OnDestroy()
    {
        LevelTimer.Cancel();
    }
}
