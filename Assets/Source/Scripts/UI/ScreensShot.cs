using UnityEngine;

public class ScreensShot : MonoBehaviour
{
#if UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ScreenCapture.CaptureScreenshot("photo.png");
        }

    }
#endif

}
