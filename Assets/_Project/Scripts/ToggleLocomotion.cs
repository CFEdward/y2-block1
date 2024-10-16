using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleLocomotion : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Update()
    {
        gameObject.SetActive(SceneManager.GetActiveScene().buildIndex != 1);
    }
}
