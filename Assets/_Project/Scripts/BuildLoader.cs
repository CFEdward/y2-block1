using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildLoader : MonoBehaviour
{
    private void Awake()
    {
        if (!Application.isEditor) LoadPersistent();
    }

    private void LoadPersistent()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
}
