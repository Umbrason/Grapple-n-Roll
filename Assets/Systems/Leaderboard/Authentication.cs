using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class Authentication
{
#if UNITY_STANDALONE
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    static async void Initialize()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        PlayerOptions.OnNameChanged += ChangePlayerName;

    }

    static async void ChangePlayerName(string playerName) => await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
}
