
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLevelLoader : MonoBehaviour
{
#if !UNITY_EDITOR
    void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
#endif
}
