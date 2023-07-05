
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public static class Leaderboard
{
    private const string leaderboardIdPrefix = "Times-";
    public static async void AddScore(float time, int levelID = 0)
    {
        var leaderboardId = $"{leaderboardIdPrefix}{levelID}";
        var playerEntry = await LeaderboardsService.Instance
        .AddPlayerScoreAsync(leaderboardId, time);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    public static async Task<LeaderboardScores> GetScoresAroundPlayer(int rangeLimit, int levelID = 0)
    {
        var leaderboardId = $"{leaderboardIdPrefix}{levelID}";
        return await LeaderboardsService.Instance.GetPlayerRangeAsync(
            leaderboardId,
            new GetPlayerRangeOptions { RangeLimit = rangeLimit }
        );
    }
}
