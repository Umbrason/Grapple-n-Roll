
using UnityEngine;

public class TimerStartZone : MonoBehaviour
{
    void OnTriggerExit(Collider collider)
    {
        if (LevelTimer.Running || collider.GetComponentInParent<TimedObject>() == null) return; //not the timed object
        LevelTimer.StartTimer();
    }
}
