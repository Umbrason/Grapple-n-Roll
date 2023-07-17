
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public static class Leaderboard
{
    private const string leaderboardId = "Times-0";
    public static async void AddScore(float time)
    {
        if(Godmode.hasBeenUsed) return; //no cheating!
        var playerEntry = await LeaderboardsService.Instance
        .AddPlayerScoreAsync(leaderboardId, time);
    }

    public static async Task<List<Unity.Services.Leaderboards.Models.LeaderboardEntry>> GetScoresAroundPlayer(int rangeLimit)
    {
        var scores = await LeaderboardsService.Instance.GetPlayerRangeAsync(
            leaderboardId,
            new GetPlayerRangeOptions { RangeLimit = rangeLimit }
        );
        return scores.Results;
    }

    public static async Task<List<Unity.Services.Leaderboards.Models.LeaderboardEntry>> GetBestScores(int rangeLimit, int offset)
    {
        var scores = await LeaderboardsService.Instance.GetScoresAsync(
            leaderboardId,
            new GetScoresOptions { Limit = rangeLimit, Offset = offset }
        );
        return scores.Results;
    }
}
