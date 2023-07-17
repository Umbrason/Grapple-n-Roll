
using UnityEngine;

public class TimePublisher : MonoBehaviour
{
    void Start()
    {
        LevelTimer.OnFinish += PublishTimeIfPB;
    }

    void OnDestroy()
    {
        LevelTimer.OnFinish -= PublishTimeIfPB;
    }

    private void PublishTimeIfPB(float time)
    {        
        Leaderboard.AddScore(time);
    }
}
