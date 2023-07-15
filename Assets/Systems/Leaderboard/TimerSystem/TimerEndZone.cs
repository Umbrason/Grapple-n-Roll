
using UnityEngine;

public class TimerEndZone : MonoBehaviour
{    
    void OnTriggerEnter(Collider collider)
    {
        if (!LevelTimer.Running || collider.GetComponentInParent<TimedObject>() == null) return; //not the timed object
        LevelTimer.StopTimer();
    }
}
