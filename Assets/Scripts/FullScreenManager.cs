using UnityEngine;

public class FullscreenManager : MonoBehaviour
{
    void Start()
    {
        // Toggle fullscreen mode after the scene loads
        Screen.fullScreen = !Screen.fullScreen;
    }

    void Update()
    {
        // Ensure fullscreen mode is applied
        if (Screen.fullScreen == false)
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
