
using TMPro;
using UnityEngine;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] private KillboxTarget killboxTarget;
    [SerializeField] private TMP_Text DeathsText;

    [SerializeField] private CoinCollector coinCollector;
    [SerializeField] private TMP_Text CoinsCollected;

    [SerializeField] private TMP_Text Time;

    public void OnFinishLevel(float time)
    {
        gameObject.SetActive(true);


        DeathsText.text = killboxTarget.ResetCounter.ToString();

        CoinsCollected.text = $"{coinCollector.collectedCoins} / {Coin.RemainingCoins + coinCollector.collectedCoins}";

        var minutes = Mathf.Floor(time / 60f);
        var seconds = time % 60;
        Time.text = minutes > 0 ?
                 $"{minutes.ToString()}m {(seconds.ToString("0."))}s"
               : $"{(seconds.ToString("0.00"))}s";
    }

    void Start()
    {
        LevelTimer.OnFinish += OnFinishLevel;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        LevelTimer.OnFinish -= OnFinishLevel;
    }
}
