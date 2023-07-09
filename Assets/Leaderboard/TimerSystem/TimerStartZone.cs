
using UnityEngine;

public class TimerStartZone : MonoBehaviour
{
    void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponentInParent<TimedObject>() == null) return; //not the timed object
        LevelTimer.StartTimer();
    }
}
