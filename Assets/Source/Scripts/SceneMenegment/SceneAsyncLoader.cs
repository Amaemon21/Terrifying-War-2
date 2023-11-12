using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneAsyncLoader : MonoBehaviour
{
    [SerializeField] private bool startApp = false;
    [SerializeField] private Image loadingBar = null;
    [SerializeField] private Image loadingCircle = null;
    [SerializeField] private Text loadingProgressText = null;

    public int LoadingSceneID = 0;
    
    private void Start()
    {
        if (startApp) LoadingSceneID = 1;
        Time.timeScale = 1;
        StartCoroutine(AsyncLoad());
    }

    private IEnumerator AsyncLoad()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1.0f);

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(LoadingSceneID);

        while (!loadingOperation.isDone)
        {
            float progress = loadingOperation.progress / 0.9f;

            loadingBar.fillAmount = progress;
            loadingCircle.fillAmount = progress;    
            loadingProgressText.text = string.Format("{0:0}%", progress * 100);

            yield return null;
        }
    }
}