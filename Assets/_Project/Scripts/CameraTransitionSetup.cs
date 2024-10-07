using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTransitionSetup : MonoBehaviour
{
    private Camera mainCamera = null;
    private readonly Color passthroughColor = new Color(0f, 0f, 0f, 0f);

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        SceneLoader.Instance.OnLoadEnd.AddListener(ChangeBackground);
    }

    private void OnDisable()
    {
        SceneLoader.Instance.OnLoadEnd.RemoveListener(ChangeBackground);
    }

    private void ChangeBackground()
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
}
