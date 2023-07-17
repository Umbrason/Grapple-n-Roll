using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLevelLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
