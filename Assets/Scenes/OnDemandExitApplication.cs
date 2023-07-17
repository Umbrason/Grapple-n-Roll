
using UnityEngine;

public class OnDemandExitApplication : MonoBehaviour
{
    public void ExitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
