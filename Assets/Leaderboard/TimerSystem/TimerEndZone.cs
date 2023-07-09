
using UnityEngine;

public class TimerEndZone : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponentInParent<TimedObject>() == null) return; //not the timed object
        LevelTimer.StopTimer();
    }
}
