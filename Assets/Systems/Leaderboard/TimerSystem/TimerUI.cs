
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    void Start()
    {
        LevelTimer.OnStart += Show;
        LevelTimer.OnFinish += Hide;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        LevelTimer.OnStart -= Show;
        LevelTimer.OnFinish -= Hide;
    }

    void Update()
    {
        text.text = LevelTimer.CurrentTime.ToString("0.00");
    }

    void Show(float _)
    {
        gameObject.SetActive(true);
    }

    void Hide(float _)
    {
        gameObject.SetActive(false);
    }
}

