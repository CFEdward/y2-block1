using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(SceneManager.GetActiveScene().buildIndex != 1);
    }
}
