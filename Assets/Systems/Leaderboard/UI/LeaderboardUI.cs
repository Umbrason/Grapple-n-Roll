
using System.Collections;
using System.Linq;
using Unity.Services.Authentication;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private LeaderboardEntry entryPrefab;
    [SerializeField] private Transform ContentContainer;

    public enum Mode
    {
        BEST = 0, SURROUNDING = 1
    }

    private Mode mode = Mode.BEST;
    public void SetMode(Mode mode)
    {
        this.mode = mode;
        StartCoroutine(FetchScoresAndShow());
    }


    public void OnEnable()
    {
        var children = Enumerable.Range(0, ContentContainer.childCount).Select(ContentContainer.GetChild).ToArray();
        foreach (var child in children) Destroy(child.gameObject);
        StartCoroutine(FetchScoresAndShow());
    }

    private IEnumerator FetchScoresAndShow()
    {
        yield return new WaitForSeconds(.05f);
        var task = mode == Mode.SURROUNDING ? Leaderboard.GetScoresAroundPlayer(10) : Leaderboard.GetBestScores(10, 0);
        yield return new WaitUntil(() => task.IsCompleted);


        var children = Enumerable.Range(0, ContentContainer.childCount).Select(ContentContainer.GetChild).ToArray();
        foreach (var child in children) Destroy(child.gameObject);
        foreach (var score in task.Result)
        {
            var isUser = score.PlayerId == AuthenticationService.Instance.PlayerId;
            var entry = Instantiate(entryPrefab, ContentContainer);
            entry.SetData((float)score.Score, score.Rank + 1, score.PlayerName.Split('#')[0], isUser);
        }
    }

}
