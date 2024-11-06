using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleVisibility : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (gameObject.CompareTag("ShipAlien"))
            {
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            if (gameObject.CompareTag("ShipAlien"))
            {
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
