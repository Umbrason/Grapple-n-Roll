
using UnityEngine;

public static class Playerskins
{
    public const string playerPrefsSkinKey = "skin";
    private static Material[] cached_skins; //not pretty but gets the job done
    public static Material[] skins => cached_skins ??= Resources.LoadAll<Material>("Skins/");

}
