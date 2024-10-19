using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public UnityEvent OnLoadBegin = new UnityEvent();
    public UnityEvent OnLoadEnd = new UnityEvent();

    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private Camera mainCamera;
    private readonly Color passthroughColor = new Color(0f, 0f, 0f, 0f);

    private bool isLoading = false;

    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        SceneManager.sceneLoaded += SetActiveScene;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    private void ChangeCameraBackground()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = passthroughColor;
        }
        else
        {
            mainCamera.clearFlags = CameraClearFlags.Skybox;
        }
    }

    public void LoadNewScene(string sceneName)
    {
        if (!isLoading) StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        isLoading = true;

        OnLoadBegin?.Invoke();
        yield return screenFader.FadeOut();
        yield return StartCoroutine(UnloadCurrent());

        // For testing
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(LoadNew(sceneName));
        ChangeCameraBackground();
        OnLoadEnd?.Invoke();
        yield return screenFader.FadeIn();

        isLoading = false;
    }

    private IEnumerator UnloadCurrent()
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        while (!unloadOperation.isDone) yield return null;
    }

    private IEnumerator LoadNew(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!loadOperation.isDone) yield return null;
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }
}
