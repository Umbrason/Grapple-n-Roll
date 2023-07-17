
using UnityEngine;

public class ApplySkin : MonoBehaviour
{
    
    [SerializeField] private MeshRenderer mr;

    void Start()
    {
        var skinIndex = PlayerPrefs.GetInt(Playerskins.playerPrefsSkinKey);
        mr.material = Playerskins.skins[skinIndex];
        
    }
}
