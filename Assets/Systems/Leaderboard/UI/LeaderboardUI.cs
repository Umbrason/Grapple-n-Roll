
using System.Collections;
using System.Linq;
using Unity.Services.Authentication;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private LeaderboardEntry entryPrefab;
    [SerializeField] private Transform ContentContainer;

    public void OnFinishLevel(float time)
    {
        gameObject.SetActive(true);
        var children = Enumerable.Range(0, ContentContainer.childCount).Select(ContentContainer.GetChild).ToArray();
        foreach (var child in children) Destroy(child.gameObject);
        StartCoroutine(FetchScoresAndShow());
    }

    private IEnumerator FetchScoresAndShow()
    {
        yield return new WaitForSeconds(.05f);
        var task = Leaderboard.GetScoresAroundPlayer(10);
        yield return new WaitUntil(() => task.IsCompleted);


        foreach (var score in task.Result.Results)
        {
            var isUser = score.PlayerId == AuthenticationService.Instance.PlayerId;
            var entry = Instantiate(entryPrefab, ContentContainer);
            entry.SetData((float)score.Score, score.Rank + 1, score.PlayerName.Split('#')[0], isUser);
        }
    }

    void Start()
    {
        LevelTimer.OnFinish += OnFinishLevel;
        gameObject.SetActive(false);
    }

    void Destroy()
    {
        LevelTimer.OnFinish -= OnFinishLevel;
    }
}
