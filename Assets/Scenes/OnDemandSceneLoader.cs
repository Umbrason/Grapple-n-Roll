
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnDemandSceneLoader : MonoBehaviour
{
    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

}
